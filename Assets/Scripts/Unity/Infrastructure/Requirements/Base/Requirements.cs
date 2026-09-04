using System.Collections.Generic;
using System.Linq;
using Unity.Infrastructure.Requirements.Visitors;

namespace Unity.Infrastructure.Requirements.Base
{
    /// <summary>
    /// API для работы с коллекцией рекваерментов
    /// </summary>
    public sealed class Requirements
    {
        private readonly List<IRequirement> _requirements;

        public IReadOnlyList<IRequirement> Each => _requirements;

        public Requirements(int cap = 4) => _requirements = new List<IRequirement>(cap);

        public Requirements(List<IRequirement> requirements) 
            => _requirements = requirements;

        public bool Check(IRequirementVisitor visitor = default)
        {
            visitor ??= CommonVisitor.Instance;
            return _requirements.All(visitor.Check);
        }
        
    }
}