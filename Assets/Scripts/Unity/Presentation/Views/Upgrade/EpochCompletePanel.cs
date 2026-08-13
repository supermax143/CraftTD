using System;
using Core.Application.Models;
using Unity.Presentation.Components;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.Windows.Upgrade
{
    public class EpochCompletePanel : MonoBehaviour
    {
        public event Action OnEpochComplete;
        
        private enum State
        {
            Hidden,
            CanComplete,
            CompleteForMoney
        }
        
        [SerializeField]
        private ResourceButton _completeForMoneyButton;
        [SerializeField]
        private Button _completeButton;
        
        
        [Inject] IMainModel _model;
        
        private State _state;
        
        public void UpdateView()
        {
            UpdateState();
            UpdateUpgradeButton();
        }

        private void UpdateState()
        {
            if (_model.CurrentPlayerEpochNumber == _model.CurrentEnemyEpochNumber)
            {
                _state = State.CompleteForMoney;
            }
            else if(_model.CurrentPlayerEpochNumber < _model.CurrentEnemyEpochNumber)
            {
                _state = State.CanComplete;
            }
            else
            {
                _state = State.Hidden;
            }
        }

        private void UpdateUpgradeButton()
        {
            gameObject.SetActive(true);
            switch (_state)
            {
                case  State.Hidden:
                    gameObject.SetActive(false);
                    break;
                case  State.CanComplete:
                    _completeButton.gameObject.SetActive(true);
                    _completeForMoneyButton.gameObject.SetActive(false);
                    break;
                case  State.CompleteForMoney:
                    _completeButton.gameObject.SetActive(false);
                    _completeForMoneyButton.gameObject.SetActive(true);
                    _completeForMoneyButton.SetPrice(new Resource(ResourceType.Money, _model.GetEpochCompleteCost()));
                    break;
            }
        }
        
        public void CompleteEpoch()
        {
            _model.CompleteEpoch();
            OnEpochComplete?.Invoke();
        }
        
        public void CompleteEpochForMoney()
        {
            _model.CompleteEpochForMoney();
            OnEpochComplete?.Invoke();
        }
        
    }
}