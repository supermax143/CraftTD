using System;
using System.Collections.Generic;
using Unity.Infrastructure.Requirements.Base;

namespace Unity.Infrastructure.Requirements.Visitors
{
    public abstract class RequirementCheckerBase<TReq> : IRequirementChecker
        where TReq: class, IRequirement
    {
        
        public bool Check<T>(T requirement) where T : class, IRequirement
        {
            return Check(requirement);
        }

        protected abstract bool Check(TReq req);
      
        
    }
}