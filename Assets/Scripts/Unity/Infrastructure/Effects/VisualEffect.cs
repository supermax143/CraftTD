using System;
using System.Collections;
using UnityEngine;

namespace Unity.Infrastructure.Effects
{
    public class VisualEffect : MonoBehaviour
    {
        public event Action OnComplete;
        
        [SerializeField]
        private EffectType _effectType;
        [SerializeField]
        private float _time = 1;
        
        public EffectType Type => _effectType;

        public void Spawn()
        {
            StartCoroutine(WaitFinish());
        }

        private IEnumerator WaitFinish()
        {
            yield return new WaitForSeconds(_time);
            OnComplete?.Invoke();
        }
        
    }
}