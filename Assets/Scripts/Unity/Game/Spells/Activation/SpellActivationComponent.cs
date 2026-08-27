using Environments.Common.Scripts;
using UnityEngine;

namespace Core.Application.Spells.Activation
{
    public abstract class SpellActivationComponent : MonoBehaviour
    {
        public Vector2 TargetPosition { get; protected set; }
        public ITouchTarget Target{ get; protected set; }
        
        public bool IsActivationCheckActive { get; protected set; }
        
        public abstract void StartActivationCheck();
        public abstract void StopActivationCheck();
    }
}