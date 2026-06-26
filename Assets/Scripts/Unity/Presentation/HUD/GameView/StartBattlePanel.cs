using System;
using Core.Application.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation
{
    public class StartBattlePanel : MonoBehaviour
    {
        
        [SerializeField] 
        private Button _startBattleButton;
        [SerializeField] 
        private Button _selectPrevEpoch;
        [SerializeField] 
        private Button _selectNextEpoch;

        [Inject] private IMainModel _mainModel;

        private void Start()
        {
            _mainModel.OnEnemyEpochChanged += UpdateView;
            _mainModel.OnPlayerEpochChanged += UpdateView;
        }

        public void UpdateView()
        {
            _selectPrevEpoch.interactable = _mainModel.SelectedEnemyEpochIndex > 0;
            _selectNextEpoch.interactable = 
                _mainModel.SelectedEnemyEpochIndex < _mainModel.CurrentPlayerEpochNumber-1 && 
                _mainModel.CurrentPlayerEpochNumber < _mainModel.CurrentEnemyEpochNumber;
        }
        
        public void SelectNextEpoch()
        {
            _mainModel.SelectEnemyEpochIndex(_mainModel.CurrentEnemyEpochNumber + 1);
        }
        
        public void SelectPrevEpoch()
        {
            _mainModel.SelectEnemyEpochIndex(_mainModel.CurrentEnemyEpochNumber - 1);
        }

        private void OnDestroy()
        {
            _mainModel.OnEnemyEpochChanged -= UpdateView;
            _mainModel.OnPlayerEpochChanged -= UpdateView;
        }
    }
}