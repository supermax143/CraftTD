using Core.Application.Requirements.Base;

namespace Core.Application.Requirements.Checkers
{
    public abstract class ReqCheckerBase<TReq> : IRequirementChecker
        where TReq: class, IRequirement
    {
        
        public virtual bool Check<T>(T requirement) where T : class, IRequirement
        {
            if (requirement is not TReq req)
            {
                return false;
            }
            
            return CheckInternal(req);
        }

        
        public float GetProgress<T>(T requirement) where T : class, IRequirement
        {
            if (requirement is not TReq req)
            {
                return 0;
            }
            return GetProgressInternal(req);
        }

        protected abstract bool CheckInternal(TReq req);
      
        protected abstract float GetProgressInternal(TReq req);
    }
}