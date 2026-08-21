using System;
using Core.Application.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Game;
using Zenject;

namespace Unity.Presentation.Views.Battle
{
    /// <summary>
    /// UI компонент для управления оборонительной стойкой по тирам юнитов
    /// </summary>
    public class DefenseStanceButton : MonoBehaviour
    {
        [SerializeField] private Image _activateIcon;
        [SerializeField] private Image _deactivateIcon;
        [SerializeField] private UnitTier _tier;

        [Inject] private IMainModel _mainModel;
        [Inject] private IGameController _gameController;
        private EpochModel Epoch => _mainModel.PlayerEpoch;
        
        private UnitModel _unitModel = null;

        private void Start()
        {
            _gameController.OnDefenseStanceSwitched += OnDefenseStanceSwitched;
            Initialize();
        }

        private void Initialize()
        {
            if (Epoch.TryGetUnitModel(_tier, out var unitModel))
            {
                _unitModel = unitModel;
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_unitModel == null)
            {
                return;
            }

            var opened = _unitModel.IsDefenseStanceOpened;
            gameObject.SetActive(opened);
            if (!opened || !_gameController.TryGetDefenseStance(_tier, out var active))
            {
                return;
            }
            UpdateDefenceState(active);
        }

        public void SwitchDefence()
        {
            _gameController.SwitchDefenseStance(_tier);
        }
        
        private void OnDefenseStanceSwitched(UnitTier tier, bool active)
        {
            if (_tier != tier)
            {
                return;
            }
            UpdateDefenceState(active);
        }

        private void UpdateDefenceState(bool active)
        {
            _activateIcon.gameObject.SetActive(!active);
            _deactivateIcon.gameObject.SetActive(active);
        }
    }
}
