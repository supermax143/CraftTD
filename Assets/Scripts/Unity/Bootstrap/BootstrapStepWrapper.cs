using System.Threading.Tasks;
using Core.Application.Interfaces;

namespace Unity.Bootstrap
{
    public class BootstrapStepWrapper
    {
        private readonly IBootstrapStep _step;
        public string StepIdent { get; private set;}

        public BootstrapStepWrapper(IBootstrapStep step, string stepIdent)
        {
            _step = step;
            StepIdent = stepIdent;
        }
        
        public async Task Execute() => await _step.Initialize();
        
    }
}