using Unity.VisualScripting;
using Zenject;

namespace Unity.Infrastructure.Tutorial.Units.BaseUnits
{
    public abstract class CustomGetUnit<TResult> : Unit 
    {
        private const string RESULT = "result";
        
        [DoNotSerialize] private ValueOutput _result;
        
        private bool _initialized = false;
        
        protected abstract TResult GetResult(Flow flow);

        protected override void Definition()
        {
            _result = ValueOutput(RESULT, Initialize);
        }
        
        private TResult Initialize(Flow flow)
        {
            if (!_initialized)
            {
                var context = Variables.Object(flow.stack.self).Get<DiContainer>("context");
                context.Inject(this);
                _initialized = true;
            }

            return GetResult(flow);
        }


    }
}
