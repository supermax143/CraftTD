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
    public class PriceButton : PointerDownClicker
    {
        
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _button;
        [SerializeField] private Image _Icon;

        [Inject] private IMainModel _model;
        
        private int _price;
        
        protected override bool Active => _model.Money.Value >= _price;
        
        public Button Button => _button;

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


    }
}