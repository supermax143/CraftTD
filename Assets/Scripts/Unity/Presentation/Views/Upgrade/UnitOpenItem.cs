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
using Utils.ColorEffects;

namespace Unity.Presentation.Components
{
    public class UnitOpenItem : MonoBehaviour
    {
        public event Action<UnitTier> OnUnitOpened;
        
        [SerializeField] private UnitIcon _unitIcon;
        [SerializeField] private ResourceButton resourceButton;
        [SerializeField] private UnitTier _tier;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasEffectsController _effectsController;
        private UnitModel _unitModel;

        public UnitTier Tier => _tier;

        public UnitModel Model => _unitModel;

        private Coroutine _animationCoroutine;
        
        public async UniTask SetUnit(UnitModel unitModel)
        {
            _unitModel = unitModel;
            await _unitIcon.Initialize(unitModel.Info.UnitPrefab);
        }

        public void UpdateOpenedState()
        {
            resourceButton.SetPrice(new Resource(ResourceType.Money, _unitModel.UnlockCost));
            resourceButton.gameObject.SetActive(!_unitModel.IsUnitOpened);
            if (!_unitIcon.TryGetUnitView(out var unitView))
            {
                return;
            }

            if (_unitModel.IsUnitOpened)
            {
                unitView.Unpause();
                _effectsController.ShowGrayscale(.2f, 0);
                _effectsController.ShowBlink(2f);
            }
            else
            {
                unitView.Pause();
                _effectsController.ShowGrayscale(0, 1);
            }
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