namespace Core.Application.Quests
{
    public enum QuestState
    {
        Inactive,
        Active,
        Complete
    }

    public class QuestProgressData
    {
        public string QuestId;
        public QuestState State;
    }
}
