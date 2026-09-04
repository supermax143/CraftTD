using Unity.Infrastructure.Requirements.Base;

namespace Unity.Infrastructure.Requirements
{
    public abstract record RequirementBase : IRequirement
    {
        public bool Check(IRequirementVisitor visitor) 
            => visitor.Check(this);

       
    }
}