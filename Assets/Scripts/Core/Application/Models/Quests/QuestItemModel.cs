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
        private QuestState _state;
        private float _progress;

        public QuestItemConfig QuestConfig => _questConfig;
        public float Progress => _progress;
        public bool IsCompleted => _state == QuestState.Complete;
        public QuestState State => _state;

        public QuestItemModel(QuestItemConfig questConfig, QuestState state = QuestState.Inactive)
        {
            _questConfig = questConfig;
            _state = state;
            _progress = state == QuestState.Complete ? 1f : 0f;
        }

        public void SetProgress(float progress)
        {
            if (_progress == progress)
            {
                return;
            }

            _progress = Mathf.Clamp01(progress);
            OnProgressChanged?.Invoke();

            if (_progress >= 1f && _state != QuestState.Complete)
            {
                _state = QuestState.Complete;
                OnCompleted?.Invoke();
            }
        }

        public void SetState(QuestState state)
        {
            if (_state == state)
            {
                return;
            }

            _state = state;
            _progress = state == QuestState.Complete ? 1f : 0f;
            OnProgressChanged?.Invoke();

            if (state == QuestState.Complete)
            {
                OnCompleted?.Invoke();
            }
        }

        public void ResetProgress()
        {
            SetState(QuestState.Inactive);
        }
    }
}
