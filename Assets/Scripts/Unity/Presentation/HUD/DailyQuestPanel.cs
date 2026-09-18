using System;
using Core.Application.Models;
using Core.Application.Models.Quests;
using Core.Application.Quests;
using TMPro;
using Unity.Game;
using Unity.Infrastructure.VisualActions;
using Unity.Infrastructure.VisualActions.ActionsData;
using Unity.Presentation.Components;
using UnityEngine;
using UnityEngine.UI;
using Utils.ColorEffects;
using Zenject;

namespace Unity.Presentation.HUD
{
    /// <summary>
    /// Панель для отображения ежедневного квеста с описанием, прогрессом и наградой
    /// </summary>
    public class DailyQuestPanel : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _descriptionText;
        [SerializeField]
        private AnimatedProgressbar _progressbar;
        [SerializeField]
        private RewardContainer _rewardContainer;
        [SerializeField] 
        private Transform _claimButton;
        [SerializeField]
        private CanvasEffectsController _effectsController;
        
        [Inject] private IQuestsModel _questsModel;
        [Inject] private IActionsDispatcher _actionsDispatcher;
        
        private QuestItemModel _currentQuest;

        private void Start()
        {
            _questsModel.OnCurrentQuestChanged += CurrentQuestChangedHandler;
            CurrentQuestChangedHandler();
        }

        private void CurrentQuestChangedHandler()
        {
            if (_currentQuest != null)
            {
                _currentQuest.OnStateChange -= UpdateVIew;
                _currentQuest.OnProgressChange -= UpdateProgress;
            }
            UpdateCurrentQuest(_questsModel.CurrentQuest);
        }


        public void UpdateCurrentQuest(QuestItemModel quest)
        {
            if (quest == null)
            {
                gameObject.SetActive(false);
                return;
            }
            _currentQuest = quest;
            _currentQuest.OnStateChange += UpdateVIew;
            _currentQuest.OnProgressChange += UpdateProgress;
            UpdateVIew();
        }

        private void UpdateVIew()
        {
            bool readyToClaim = _currentQuest.State == QuestState.ReadyToClaim;
            _claimButton.gameObject.SetActive(readyToClaim);
            _progressbar.gameObject.SetActive(!readyToClaim);
            _descriptionText.text = _currentQuest.QuestConfig.Id;
            _rewardContainer.SetReward(_currentQuest.QuestConfig.Reward);
            UpdateProgress();
            if (readyToClaim)
            {
                _effectsController.StartBlink();
            }
            else
            {
                _effectsController.StopBlink();
            }
        }

        private void UpdateProgress()
        {
            _progressbar.SetProgress(_currentQuest.Progress);
        }
        
        public void ClaimReward()
        {
            if (_currentQuest.State != QuestState.ReadyToClaim)
            {
                return;
            }
            _questsModel.ClaimCurrentQuest();
            var resource = _currentQuest.QuestConfig.Reward.ToResource();
            
            /*ector2 screenPoint = RectTransformUtility.WorldToScreenPoint(
                null,
                _rewardContainer.transform.position
            );
            var pos = Camera.main.ScreenToWorldPoint(screenPoint);
            _dropManager.ShowUiDrop(resource, pos);*/
            _actionsDispatcher.AddAction(new ShowResourceDropActionData()
            {
                Resource = resource,
                StartPosition = _rewardContainer.transform.position,
                IsUiDrop = true
            });
        }

        private void OnDestroy()
        {
            _questsModel.OnCurrentQuestChanged -= CurrentQuestChangedHandler;
            if (_currentQuest != null)
            {
                _currentQuest.OnStateChange -= UpdateVIew;
                _currentQuest.OnProgressChange -= UpdateProgress;
            }
        }
    }
}
