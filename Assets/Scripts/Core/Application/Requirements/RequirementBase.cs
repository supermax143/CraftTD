using System;
using Core.Application.Requirements.Base;

namespace Core.Application.Requirements
{
    [Serializable]
    public abstract class RequirementBase : IRequirement
    {
        public bool Check(IRequirementChecker checker) 
            => checker.Check(this);
       
    }
}