using System.Collections;
using Unity.Infrastructure.Sound;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Zenject;

namespace Unity.Presentation.Components
{
    public class PointerDownClicker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] 
        private float _activationInterval = 0.1f;
        [SerializeField]
        private AudioClip _clickSound;
        [SerializeField] 
        private bool _autoClick = true;
        [SerializeField]
        private UnityEvent _onActivated;
        [SerializeField]
        private UnityEvent _onPointerDown;
        [SerializeField]
        private UnityEvent _onPointerUp;
        
        [Inject] private SoundManager _soundManager;
        
        private Coroutine _pressCoroutine;
        
        protected virtual bool Active { get; }

        public void OnPointerDown(PointerEventData eventData)
        {
            _onPointerDown?.Invoke();
            if (!_autoClick)
            {
                return;
            }
            _pressCoroutine = StartCoroutine(Activate());
        }
       
        public void OnPointerUp(PointerEventData eventData)
        {
            _onPointerUp?.Invoke();
            if (_pressCoroutine != null)
            {
                StopCoroutine(_pressCoroutine);
            }
        }
        
        private IEnumerator Activate()
        {
            yield return new WaitUntil(() => Active);
            PlayClickSound();
            _onActivated?.Invoke();
            yield return new WaitForSeconds(_activationInterval);
            _pressCoroutine = StartCoroutine(Activate());
        }
        
        private void PlayClickSound()
        {
            if (_clickSound == null)
            {
                return;
            }
            _soundManager.PlaySound(_clickSound, true);
        }
    }
}