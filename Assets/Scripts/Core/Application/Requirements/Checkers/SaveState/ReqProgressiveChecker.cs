using Core.Application.DataStorage;
using Core.Application.DataStorage.StorageItems;
using Unity.Infrastructure.Requirements.Base;
using Zenject;

namespace Unity.Infrastructure.Requirements.Visitors
{
    public abstract class ReqProgressiveChecker<TReq> : ReqCheckerBase<TReq>
        where TReq: ReqProgressive, IRequirement
    {
        [Inject] private IDataStorage _dataStorage;
        
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