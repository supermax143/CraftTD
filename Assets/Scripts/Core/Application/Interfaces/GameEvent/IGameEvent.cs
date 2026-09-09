namespace Unity.Infrastructure.GameEvents
{
    public interface IGameEvent
    {
        string Name { get; }
        bool Equal(IGameEvent other);
    }
}