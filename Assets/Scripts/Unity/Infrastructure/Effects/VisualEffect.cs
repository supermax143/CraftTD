using System;
using System.Collections;
using UnityEngine;

namespace Unity.Infrastructure.Effects
{
    public class VisualEffect : MonoBehaviour
    {
        public event Action<VisualEffect> OnComplete;
        
        [SerializeField]
        private VisualEffectType visualEffectType;
        [SerializeField]
        private float _time = 1;
        
        public VisualEffectType Type => visualEffectType;

        public virtual void Spawn()
        {
            gameObject.SetActive(true);
            StartCoroutine(WaitFinish());
        }

        private IEnumerator WaitFinish()
        {
            yield return new WaitForSeconds(_time);
            OnComplete?.Invoke(this);
        }
        
    }
}