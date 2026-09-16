using System;
using Core.Application.Models.Quests;
using Core.Application.Quests;
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
            quest.OnProgressChange += UpdateProgress;
        }

        private void UpdateVIew()
        {
            _descriptionText.text = _currentQuest.QuestConfig.Id;
            _stateText.text = _currentQuest.State.ToString();
            _rewardContainer.SetReward(_currentQuest.QuestConfig.Reward);
            UpdateProgress();
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
        }

       
    }
}
