using UnityEngine;

namespace Core.Application.Spells.Targeting
{
    public abstract class SpellTargetingComponent : MonoBehaviour
    {
        
        public Vector2 TargetPosition { get; protected set; }

        public abstract bool IsTargetingActive();
        
       
        public virtual void Activate()
        {
            
        }
        
        public virtual void Deactivate()
        {
            
        }
    }
}