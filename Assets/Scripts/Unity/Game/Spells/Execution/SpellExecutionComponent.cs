using System;
using System.Collections;
using UnityEngine;

namespace Unity.Game.Spells.Execution
{
    public abstract class SpellExecutionComponent : MonoBehaviour
    {
        public event Action OnComplete;

        [SerializeField]
        protected float _delayBeforeDestroy = 1;

        protected abstract IEnumerator DoExecute(SpellController spell);

        public void Execute(SpellController spell)
        {
            StartCoroutine(WaitExecuted(spell));
        }
       

        private IEnumerator WaitExecuted(SpellController spell)
        {
            yield return DoExecute(spell);
            OnComplete?.Invoke();
        }




    }
}