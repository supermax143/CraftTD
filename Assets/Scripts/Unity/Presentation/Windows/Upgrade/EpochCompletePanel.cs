using System;
using Core.Application.Models;
using Unity.Presentation.Components;
using UnityEngine;
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
        private PriceButton _completeForMoneyButton;
        [SerializeField]
        private PriceButton _completeButton;
        
        
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
                    _completeForMoneyButton.SetPrice(_model.GetEpochCompleteCost());
                    break;
            }
        }
        
        
        public void CompleteEpochForMoney()
        {
            _model.CompleteEpochForMoney();
            OnEpochComplete?.Invoke();
        }
        
    }
}