using Unity.Game.Attributes;
using UnityEngine;

namespace Unity.Game.Data.Attributes
{
    public abstract class GameObjectEntityAttribute : GameEntityAttribute<GameObject>
    {
        public GameObjectEntityAttribute(GameObject baseValue)
            : base(baseValue)
        {
        }

    }
}