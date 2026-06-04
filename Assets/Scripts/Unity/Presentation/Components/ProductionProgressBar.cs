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
        
        [Inject] private FoodProduction _foodProduction;
        
        private void Start()
        {
            _foodProduction.OnFoodProductionStarted += FoodProductionStartedHandler;
            _foodProduction.OnFoodProduced += FoodProducedHandler;
        }

        private void FoodProducedHandler()
        {
            _foodCountTF.text = _foodProduction.CurrentFoodCount.ToString();
        }

        private void FoodProductionStartedHandler()
        {
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