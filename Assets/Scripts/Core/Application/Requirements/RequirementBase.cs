using System;
using Unity.Infrastructure.Requirements.Base;

namespace Unity.Infrastructure.Requirements
{
    [Serializable]
    public abstract class RequirementBase : IRequirement
    {
        public bool Check(IRequirementVisitor visitor) 
            => visitor.Check(this);

       
    }
}