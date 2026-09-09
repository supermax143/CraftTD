using System;

namespace Unity.Infrastructure.GameEvents
{
    public interface IGameEventsBus
    {
        void AddListener<TEvent>(Action<TEvent> listener) where TEvent : IGameEvent;
        void RemoveListener<TEvent>(Action<TEvent> listener) where TEvent : IGameEvent;
        void TriggerEvent(IGameEvent gameEvent);
        event Action<IGameEvent> OnGameEvent;
    }
}