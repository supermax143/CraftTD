using System;
using Core.Application.Quests;
using UnityEngine;

namespace Core.Application.Models.Quests
{
    public class QuestItemModel
    {
        public event Action OnProgressChanged;
        public event Action OnCompleted;

        private readonly QuestItemConfig _questConfig;
        private float _progress;

        public QuestItemConfig QuestConfig => _questConfig;
        public float Progress => _progress;
        public bool IsCompleted => _progress >= 1f;

        public QuestItemModel(QuestItemConfig questConfig, float progress = 0f)
        {
            _questConfig = questConfig;
            _progress = progress;
        }

        public void SetProgress(float progress)
        {
            if (_progress == progress)
            {
                return;
            }

            _progress = Mathf.Clamp01(progress);
            OnProgressChanged?.Invoke();

            if (IsCompleted)
            {
                OnCompleted?.Invoke();
            }
        }

        public void ResetProgress()
        {
            SetProgress(0f);
        }
    }
}
