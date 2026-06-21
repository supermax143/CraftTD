using System;
using Core.Application.Models;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.Components
{
    public class PriceButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _button;
        [SerializeField] private Image _Icon;
     
        [Inject] private IMainModel _model;
        
        private int _price;

        public Button Button => _button;

        private void Start()
        {
        }

        public void SetPrice(int price)
        {
            _price = price;
            UpdateView();
        }

        private void UpdateView()
        {
            var active = _model.Money.Value >= _price;
            _priceText.text = _price.ToString();
            var color = active ? Color.black : Color.red;
            _priceText.color = color;
            _button.interactable = active;
        }
    }
}