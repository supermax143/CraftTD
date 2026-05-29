using System;

namespace Unity.Infrastructure.GameEvents
{
    public class GameEventsBus
    {
        public Action<GameEvent> OnGameEvent;
        
        
        public void TriggerEvent(GameEvent gameEvent) => OnGameEvent?.Invoke(gameEvent);
    }
}