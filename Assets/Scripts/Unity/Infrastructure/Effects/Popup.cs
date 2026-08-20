using System;
using System.Collections;
using UnityEngine;

namespace Unity.Infrastructure.Effects
{
    public class Popup : MonoBehaviour
    {
        public event Action<Popup> OnComplete;
        
        [SerializeField]
        private PopupType visualEffectType;
        [SerializeField]
        private float _time = 1;
        
        public PopupType Type => visualEffectType;

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