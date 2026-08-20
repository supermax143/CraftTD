using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Infrastructure.Effects
{
    public class Popup : MonoBehaviour
    {
        public event Action<Popup> OnComplete;
        
        [FormerlySerializedAs("visualEffectType")]
        [SerializeField]
        private PopupType _popupType;
        [SerializeField]
        protected float _time = 1;
        
        public PopupType Type => _popupType;

        public virtual void Spawn()
        {
            gameObject.SetActive(true);
            StartCoroutine(WaitFinish());
        }

        protected virtual IEnumerator WaitFinish()
        {
            yield return new WaitForSeconds(_time);
            DispatchComplete();
        }

        protected void DispatchComplete()
        {
            OnComplete?.Invoke(this);
        }
    }
}