using TMPro;
using Unity.Presentation.Components;
using UnityEngine;
using UnityEngine.UI;

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
        private ResourceContainer _rewardContainer;
        
        private int _targetValue;
        private int _currentValue;

        public void SetQuest(string description, int target, int current, Core.Application.Models.Resource reward)
        {
            _descriptionText.text = description;
            _targetValue = target;
            _currentValue = current;
            
            UpdateProgress();
            _rewardContainer.SetResource(reward);
        }

        public void UpdateProgress(int currentValue)
        {
            _currentValue = currentValue;
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            float progress = _targetValue > 0 ? (float)_currentValue / _targetValue : 0f;
            _progressbar.SetProgress(progress);
        }
    }
}
