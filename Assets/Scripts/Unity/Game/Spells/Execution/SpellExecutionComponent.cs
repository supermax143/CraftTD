using System;
using UnityEngine;

namespace Unity.Game.Spells.Execution
{
    public abstract class SpellExecutionComponent : MonoBehaviour
    {
        public event Action OnComplete;

        [SerializeField]
        protected float _delayBeforeDestroy = 1;
        
        public  abstract void Execute(SpellController spell);

        public void Complete()
        {
            OnComplete?.Invoke();
        }
    }
}