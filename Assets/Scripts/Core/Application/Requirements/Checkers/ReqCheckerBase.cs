using Core.Application.Requirements.Base;

namespace Core.Application.Requirements.Checkers
{
    public abstract class ReqCheckerBase<TReq> : IRequirementChecker
        where TReq: class, IRequirement
    {
        
        public bool Check<T>(T requirement) where T : class, IRequirement
        {
            return Check(requirement);
        }

        protected abstract bool Check(TReq req);
      
        
    }
}