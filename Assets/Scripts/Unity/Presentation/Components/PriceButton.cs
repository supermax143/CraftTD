using Core.Application.Models;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Zenject;
using Image = Microsoft.Unity.VisualStudio.Editor.Image;

namespace Unity.Presentation.Components
{
    public class PriceButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _button;
        [SerializeField] private Image _Icon;
     
        [Inject] private IMainModel _model;
        
        private int _price;
        
        public void SetPrice(int price)
        {
            _price = price;
            UpdateView();
        }

        private void UpdateView()
        {
            var color = _price > 0 ? Color.red : Color.white;
            _priceText.color = color;
            _button.interactable = _model.Money >= _price;
        }
    }
}