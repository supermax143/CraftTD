using System;
using System.Collections.Generic;
using Zenject;

namespace Unity.Infrastructure.GameEvents
{
    public class GameEventsBus : IGameEventsBus
    {
        public event Action<IGameEvent> OnGameEvent;
        
        [Inject] private DiContainer _container;
        
        private readonly Dictionary<Type, List<Delegate>> _typeToListeners = new();

        public void AddListener<TEvent>(Action<TEvent> listener) where TEvent : IGameEvent
        {
            var type = typeof(TEvent);
            
            if (!_typeToListeners.TryGetValue(type, out var listeners))
            {
                listeners = new List<Delegate>();
                _typeToListeners[type] = listeners;
            }

            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void RemoveListener<TEvent>(Action<TEvent> listener) where TEvent : IGameEvent
        {
            var type = typeof(TEvent);

            if (_typeToListeners.ContainsKey(type))
            {
                _typeToListeners[type].Remove(listener);
            }
        }
        

        public void TriggerEvent(IGameEvent gameEvent)
        {
            OnGameEvent?.Invoke(gameEvent);
            
            var type = gameEvent.GetType();

            if (_typeToListeners.TryGetValue(type, out var listeners))
            {
                for (int i = 0; i < listeners.Count; i++)
                {
                    listeners[i].DynamicInvoke(gameEvent);
                }
            }
        }
    }
}