namespace Core.Application.Quests
{
    public enum QuestState
    {
        Inactive,
        Active,
        ReadyToClaim,
        Complete
    }

    public class QuestProgressData
    {
        public string QuestId;
        public QuestState State;
    }
}
