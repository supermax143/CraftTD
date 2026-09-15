using System;

namespace Core.Application.Models.Quests
{
    public interface IQuestsModel
    {
        event Action OnCurrentQuestChanged;
        QuestItemModel CurrentQuest { get; }
        event Action OnQuestsReset;
    }
}