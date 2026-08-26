using UnityEngine;

namespace Unity.Game.Spells.Execution
{
    public abstract class SpellExecutionComponent : MonoBehaviour
    {
        public  abstract void Execute(SpellController spell);
    }
}