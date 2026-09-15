using System;
using Core.Application.Models.Quests;
using TMPro;
using Unity.Presentation.Components;
using UnityEngine;
using UnityEngine.UI;
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
        private TMP_Text _stateText;
        
        [Inject] private IQuestsModel _questsModel;
        
        private QuestItemModel _currentQuest;

        private void Start()
        {
            SetQuest(_questsModel.CurrentQuest);
        }


        public void SetQuest(QuestItemModel quest)
        {
            _currentQuest = quest;
            UpdateVIew();
            quest.OnStateChange += UpdateVIew;
            
            /*
            UpdateProgress();
            _rewardContainer.SetResource(reward);*/
        }

        private void UpdateVIew()
        {
            _descriptionText.text = _currentQuest.QuestConfig.Id;
            _stateText.text = _currentQuest.State.ToString();
            _rewardContainer.SetReward(_currentQuest.QuestConfig.Reward);
        }

        /*public void UpdateProgress(int currentValue)
        {
            _currentValue = currentValue;
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            float progress = _targetValue > 0 ? (float)_currentValue / _targetValue : 0f;
            _progressbar.SetProgress(progress);
        }*/
    }
}
