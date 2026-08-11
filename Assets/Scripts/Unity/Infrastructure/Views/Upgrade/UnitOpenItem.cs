using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using Environments.Land.Scripts.Runtime.GUI;
using TMPro;
using Unity.Game;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Presentation.Components
{
    public class UnitOpenItem : MonoBehaviour
    {
        public event Action<UnitTier> OnUnitOpened;
        
        [SerializeField] private UnitIcon _unitIcon;
        [SerializeField] private PriceButton _priceButton;
        [SerializeField] private UnitTier _tier;
        [SerializeField] private CanvasGroup _canvasGroup;
        private UnitModel _unitModel;

        public UnitTier Tier => _tier;

        private Coroutine _animationCoroutine;
        
        public async UniTask SetUnit(UnitModel unitModel)
        {
            _unitModel = unitModel;
            await _unitIcon.Initialize(unitModel.Info.UnitPrefab);
            _priceButton.SetPrice(new Resource(ResourceType.Money, _unitModel.UnlockCost));
            _priceButton.gameObject.SetActive(!_unitModel.IsUnitOpened);
        }

        public void Show(float time)
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            
            if (time == 0)
            {
                _canvasGroup.alpha = 1;
                return;
            }
            
            _animationCoroutine = StartCoroutine(AnimateShowHide(time, 1f));
        }

        public void Hide(float time)
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }

            if (time == 0)
            {
                _canvasGroup.alpha = 0;
                return;
            }
            
            _animationCoroutine = StartCoroutine(AnimateShowHide(time, 0f));
        }
        
        private IEnumerator AnimateShowHide(float time, float targetAlpha)
        {
            var startAlpha = _canvasGroup.alpha;
            var timer = new Timer();
            timer.Start(time);
            while (!timer.IsComplete)
            {
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer.Progress);
                yield return  null;
            }
            _canvasGroup.alpha = targetAlpha;
        }
        
        public void OpenUnit()
        {
            OnUnitOpened?.Invoke(_tier);
        }
       
    }
}