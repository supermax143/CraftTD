using System;
using System.Collections;
using Core.Application.Models;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.Components
{
    public class PriceButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public UnityEvent OnActivated;
        
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _button;
        [SerializeField] private Image _Icon;
        [SerializeField] private float _activationInterval = 0.1f;
        
        [Inject] private IMainModel _model;
        
        private int _price;
        private Coroutine _pressCoroutine;
        
        public Button Button => _button;
        
        private bool Active => _model.Money.Value >= _price;
        
        public void SetPrice(int price)
        {
            _price = price;
            UpdateView();
        }

        private void UpdateView()
        {
            _priceText.text = _price.ToString();
            var color = Active ? Color.black : Color.red;
            _priceText.color = color;
            _button.interactable = Active;
        }
        
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
            OnActivated?.Invoke();
            _pressCoroutine = StartCoroutine(Activate());
        }

    }
}