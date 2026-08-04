using System;
using Core.Application.Models;
using Unity.Game;
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
        [Inject] private IGameController _gameController;

        private void Start()
        {
            _mainModel.OnEnemyEpochChanged += UpdateView;
            _mainModel.OnPlayerEpochChanged += UpdateView;
            UpdateView();
        }

        public void UpdateView()
        {
            _selectPrevEpoch.interactable =
                _mainModel.SelectedEnemyEpochIndex > 0;
            _selectNextEpoch.interactable = 
                _mainModel.SelectedEnemyEpochIndex < (_mainModel.CurrentEnemyEpochNumber-1); 
        }
        
        public void SelectNextEpoch()
        {
            //_mainModel.SelectEnemyEpochIndex(_mainModel.SelectedEnemyEpochIndex + 1);
            _gameController.SelectNextEnemyEpoch();
        }
        
        public void SelectPrevEpoch()
        {
            //_mainModel.SelectEnemyEpochIndex(_mainModel.SelectedEnemyEpochIndex - 1);
            _gameController.SelectPrevEnemyEpoch();
        }

        private void OnDestroy()
        {
            _mainModel.OnEnemyEpochChanged -= UpdateView;
            _mainModel.OnPlayerEpochChanged -= UpdateView;
        }
    }
}