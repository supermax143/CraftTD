using System;
using Zenject;

namespace Unity.Infrastructure.GameEvents
{
    public class GameEventsBus : IGameEventsBus
    {
        public Action<GameEvent> OnGameEvent;
        
        [Inject] private DiContainer _container;
        
        
        
        public void TriggerEvent(GameEvent gameEvent) => OnGameEvent?.Invoke(gameEvent);
    }
}