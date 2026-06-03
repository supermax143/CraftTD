using Unity.Game.Attributes;

namespace Unity.Game
{
    public abstract class GameEntityData
    {
        public abstract GameEntityAttribute[] GetAllAttributes();

        
        public bool TryGetAttribute<T>(out T attribute) where T : GameEntityAttribute
        {
            var allAttributes = GetAllAttributes();
            foreach (var attr in allAttributes)
            {
                if (attr is T typedAttr)
                {
                    attribute = typedAttr;
                    return true;
                }
            }

            attribute = default;
            return false;
        }
    }
}