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

        public QuestItemConfig QuestConfig => _questConfig;
        public bool IsCompleted => _state == QuestState.Complete;
        public QuestState State => _state;

        public QuestItemModel(QuestItemConfig questConfig, QuestState state = QuestState.Inactive)
        {
            _questConfig = questConfig;
            _state = state;
        }
        

        public void SetState(QuestState state)
        {
            if (_state == state)
            {
                return;
            }

            _state = state;
            OnProgressChanged?.Invoke();

            if (state == QuestState.Complete)
            {
                OnCompleted?.Invoke();
            }
        }
        
    }
}
