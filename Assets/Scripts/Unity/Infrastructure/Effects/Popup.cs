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
        private float _time = 1;
        
        public PopupType Type => _popupType;

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