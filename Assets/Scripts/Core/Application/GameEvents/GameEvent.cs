using System.Collections.Generic;

namespace Unity.Infrastructure.GameEvents
{
    public class GameEvent : IGameEvent
    {
        public string Name { get; protected set; }

        protected readonly Dictionary<string, string> _params = new();

        public GameEvent() { }
        
        public GameEvent(string name, Dictionary<string, string> @params)
        {
            Name = name;
            _params = @params;
        }
        
        public bool Equal(IGameEvent other)
        {
            var otherEvent = other as GameEvent;
            if (otherEvent == null)
            {
                return false;
            }
            
            if (otherEvent.Name != Name)
            {
                return false;
            }

            if (_params.Count != otherEvent._params.Count)
            {
                return false;
            }

            foreach (var kvp in _params)
            {
                if (_params[kvp.Key] != otherEvent._params[kvp.Key])
                {
                    return false;
                }
            }
            return true;
        }
    }
}