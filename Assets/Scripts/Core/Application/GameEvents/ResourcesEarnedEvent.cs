using Core.Application.Models;

namespace Unity.Infrastructure.GameEvents
{
    public class ResourcesEarnedEvent : GameEvent
    {
        public Resource Resource { get; }

        
        public ResourcesEarnedEvent(Resource resource) : this(resource.Type, resource.Value)
        {
            
        }
        
        public ResourcesEarnedEvent(ResourceType resourceType, int count)
        {
            Resource = new Resource(resourceType, count);
            
            Name = GameEventTypes.ResourcesEarned;
            _params[nameof(resourceType)] = resourceType.ToString();
            _params[nameof(count)] = count.ToString();
        }
    }
}
