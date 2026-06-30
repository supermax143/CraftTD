using Unity.Game;
using Unity.Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.Windows.Pause
{
    /// <summary>
    /// Окно паузы игры
    /// </summary>
    [Window(nameof(PauseWindow))]
    public class PauseWindow : WindowBase
    {
        private IGameController _gameController;

        public void Initialize(IGameController gameController)
        {
            _gameController = gameController;
            _gameController.Pause(true);
        }
        

        public void ResumeGame()
        {
            _gameController.Pause(false);
            Hide();
        }

        public void EndGame()
        {
            _gameController.Pause(false);
            _gameController.EndGame(Faction.Enemy);
            Hide();
        }
    }
}
