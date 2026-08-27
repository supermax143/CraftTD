using Core.Application.Spells.Activation;
using Unity.Game;
using UnityEngine;
using Zenject;

namespace Core.Application.Spells.Targeting
{
    public class FieldTargetingActivationComponent : SpellActivationComponent
    {
        [Inject] private GameController _gameController;
        

        public override void StartActivationCheck()
        {
            IsActivationCheckActive = true;
            _gameController.LocationSwitcher.CurrentLocation.ActivateRoadClick(true);
            _gameController.LocationSwitcher.CurrentLocation.OnRoadClick += OnRoadClick;
        }

        private void OnRoadClick(Vector2 position)
        {
            TargetPosition = position;
            IsActivationCheckActive = false;
        }

        public override void StopActivationCheck()
        {
            _gameController.LocationSwitcher.CurrentLocation.ActivateRoadClick(false);
            _gameController.LocationSwitcher.CurrentLocation.OnRoadClick -= OnRoadClick;
        }
    }
}
