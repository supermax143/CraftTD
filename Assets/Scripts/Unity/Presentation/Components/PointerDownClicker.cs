using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Unity.Presentation.Components
{
    public abstract class PointerDownClicker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float _activationInterval = 0.1f;
        [SerializeField] private UnityEvent _onActivated;
        
        private Coroutine _pressCoroutine;
        
        protected abstract bool Active {get;}

        public void OnPointerDown(PointerEventData eventData)
        {
            _pressCoroutine = StartCoroutine(Activate());
        }
       
        public void OnPointerUp(PointerEventData eventData)
        {
            if (_pressCoroutine != null)
            {
                StopCoroutine(_pressCoroutine);
            }
        }
        
        private IEnumerator Activate()
        {
            yield return new WaitForSeconds(_activationInterval);
            yield return new WaitUntil(() => Active);
            _onActivated?.Invoke();
            _pressCoroutine = StartCoroutine(Activate());
        }
    }
}