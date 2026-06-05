using System.Collections;
using TMPro;
using Unity.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.Components
{
    public class ProductionProgressBar : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _foodCountTF;
        [SerializeField]
        private Image _fillImage;
        
        [Inject] private IFoodProduction _foodProduction;
        
        private void Start()
        {
            _foodProduction.OnFoodProduced += FoodProducedHandler;
            _foodCountTF.text = _foodProduction.CurrentFoodCount.ToString();
            if (!_foodProduction.Started)
            {
                _foodProduction.OnFoodProductionStarted += FoodProductionStartedHandler;
            }
            else
            {
                FoodProductionStartedHandler();
            }
        }

        private void FoodProducedHandler()
        {
            _foodCountTF.text = _foodProduction.CurrentFoodCount.ToString();
        }

        private void FoodProductionStartedHandler()
        {
            _foodProduction.OnFoodProductionStarted -= FoodProductionStartedHandler;
            StartCoroutine(UpdateProgress());
        }
        
        private IEnumerator UpdateProgress()
        {
            while (true)
            {
                _fillImage.fillAmount = _foodProduction.CurProgress;
                yield return null;
            }
        }
        
    }
}