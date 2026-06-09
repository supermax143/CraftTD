using Unity.Game.Attributes;
using UnityEngine;

namespace Unity.Game.Data.Attributes
{
    public class GameObjectEntityAttribute: GameEntityAttribute<GameObject>
    {
        public GameObjectEntityAttribute(GameObject value, GameEntityAttributeKind kind) 
            : base(value, kind) { }
    }
}