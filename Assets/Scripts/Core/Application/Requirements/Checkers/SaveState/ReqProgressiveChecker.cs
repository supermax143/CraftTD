using Core.Application.DataStorage;
using Unity.Infrastructure.Requirements.Base;
using Zenject;

namespace Unity.Infrastructure.Requirements.Visitors
{
    public abstract class ReqProgressiveChecker<TReq> : RequirementCheckerBase<TReq>
        where TReq: ReqProgressive, IRequirement
    {
        [Inject] private IDataStorage _dataStorage;
        
        protected int GetProgress(TReq req)
        {
            //TODO: Брать значение из RequirementsProgressData по ReqProgressive.GetProgressSaveIdent
        }
        
        protected int IncrementProgress(TReq req, int increment)
        {
            //TODO: сохранять значение из RequirementsProgressData по ReqProgressive.GetProgressSaveIdent
        }
    }
}