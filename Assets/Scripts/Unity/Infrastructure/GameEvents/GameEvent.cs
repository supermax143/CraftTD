using System.Collections.Generic;

namespace Unity.Infrastructure.GameEvents
{
    public class GameEvent
    {
        public string Name { get; protected set; }

        protected readonly Dictionary<string, string> _params = new();

        public GameEvent() { }
        
        public GameEvent(string name, Dictionary<string, string> parametrs)
        {
            Name = name;
            _params = parametrs;
        }
        
        public bool Equal(GameEvent other)
        {
            if (other.Name != Name)
            {
                return false;
            }

            if (_params.Count != other._params.Count)
            {
                return false;
            }

            foreach (var kvp in _params)
            {
                if (_params[kvp.Key] != other._params[kvp.Key])
                {
                    return false;
                }
            }
            return true;
        }
    }
}