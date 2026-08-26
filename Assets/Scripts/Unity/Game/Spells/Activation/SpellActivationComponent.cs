using UnityEngine;

namespace Core.Application.Spells.Activation
{
    public abstract class SpellActivationComponent : MonoBehaviour
    {
        
        public bool IsActivationCheckActive { get; protected set; }
        
        public abstract void StartActivationCheck();
    }
}