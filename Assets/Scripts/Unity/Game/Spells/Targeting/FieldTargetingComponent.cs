using Unity.Game;
using UnityEngine;
using Zenject;

namespace Core.Application.Spells.Targeting
{
    public class FieldTargetingComponent : SpellTargetingComponent
    {
        [Inject] private GameController _gameController;

        
        public override bool IsTargetingActive()
        {
            return TargetPosition == default;
        }

        public override void Activate()
        {
            _gameController.LocationSwitcher.CurrentLocation.ActivateRoadClick(true);
            _gameController.LocationSwitcher.CurrentLocation.OnRoadClick += OnRoadClick;
        }

        private void OnRoadClick(Vector2 position)
        {
            TargetPosition = position;
        }

        public override void Deactivate()
        {
            _gameController.LocationSwitcher.CurrentLocation.ActivateRoadClick(false);
            _gameController.LocationSwitcher.CurrentLocation.OnRoadClick -= OnRoadClick;
        }
    }
}
