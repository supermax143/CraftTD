using Core.Application.DataStorage;
using Core.Application.Requirements.Base;
using Core.Application.Requirements.SaveState;
using Zenject;

namespace Core.Application.Requirements.Checkers.SaveState
{
    public abstract class ReqProgressiveChecker<TReq, TEvent> : ReqCheckerBase<TReq>
        where TReq: ReqProgressive, IRequirement
    {
        [Inject] private IDataStorage _dataStorage;
        
        protected TEvent _event;

        protected ReqProgressiveChecker(TEvent @event)
        {
            _event = @event;
        }
        
        public void UpdateEvent(TEvent @event)
        {
            _event = @event;
        }
        
        protected int GetProgress(TReq req)
        {
            return _dataStorage.RequirementsProgress.GetRequirementProgress(req.GetProgressSaveIdent());
        }
        
        protected void IncrementProgress(TReq req, int increment)
        {
            var currentProgress = GetProgress(req);
            var newProgress = currentProgress + increment;
            _dataStorage.RequirementsProgress.SetRequirementProgress(req.GetProgressSaveIdent(), newProgress);
        }
    }
}