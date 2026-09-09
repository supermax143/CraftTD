using Unity.Infrastructure.Requirements.Base;

namespace Unity.Infrastructure.Requirements.Visitors
{
    public abstract class ReqProgressiveChecker<TReq> : RequirementCheckerBase<TReq>
        where TReq: ReqProgressive, IRequirement
    {
        protected int GetProgress(TReq req)
        {
            return req.GetProgressSaveIdent();
        }
    }
}