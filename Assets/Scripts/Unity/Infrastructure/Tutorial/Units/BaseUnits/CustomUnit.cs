using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.Tutorial.Units.BaseUnits
{
    public abstract class CustomUnit : Unit
    {
        [DoNotSerialize] private readonly Dictionary<string, IUnitValuePort> _valuePorts = new();

        [DoNotSerialize] [PortLabelHidden] private ControlInput _enter;
        [DoNotSerialize] [PortLabelHidden] private ControlOutput _exit;

        protected IEnumerable<ValueInput> EachInput => _valuePorts.Values.OfType<ValueInput>();
        protected IEnumerable<ValueInput> EachNonEmptyInput => EachInput.Where(HasArg);


        protected abstract IEnumerable<IUnitValuePort> DefineValuePortsInternal();
        protected abstract void OnExecute(Flow flow);

        protected sealed override void Definition()
        {
            _enter = ControlInput(nameof(_enter), Execute);
            _exit = ControlOutput(nameof(_exit));
            DefineValuePorts();
            Succession(_enter, _exit);
        }

        private void DefineValuePorts()
        {
            _valuePorts.Clear();
            foreach (var port in DefineValuePortsInternal())
            {
                _valuePorts[port.key] = port;
            }
        }

        protected T GetValue<T>(Flow flow, string key)
        {
            return flow.GetValue<T>(GetPort<ValueInput>(key));
        }

        protected void SetValue<T>(Flow flow, string key, T value)
        {
            flow.SetValue(GetPort<ValueOutput>(key), value);
        }

        private T GetPort<T>(string key) where T : IUnitValuePort
        {
            if (!_valuePorts.TryGetValue(key, out var port))
                throw new InvalidOperationException($"Port with key {key} wasn't defined");

            if (port is not T result)
                throw new InvalidOperationException($"Can't get value from port {key}");

            return result;
        }

        private ControlOutput Execute(Flow flow)
        {
            try
            {
                Initialize(flow);
                OnExecute(flow);
                return _exit;
            }
            catch (Exception e)
            {
                Debug.Log(
                    $"Unit: {GetType().Name}" +
                    $"Error: {e.Message}.\n" +
                    $"Stack: {e.StackTrace}");
                throw;
            }
        }

        protected virtual void Initialize(Flow flow)
        {
            var context = Variables.Object(flow.stack.self).Get<DiContainer>("context");
            context.Inject(this);
        }

        protected bool HasArg(string key)
        {
            var port = GetPort<ValueInput>(key);
            return HasArg(port);
        }

        private static bool HasArg(ValueInput port)
        {
            return port.connection is { sourceExists: true };
        }
    }
}