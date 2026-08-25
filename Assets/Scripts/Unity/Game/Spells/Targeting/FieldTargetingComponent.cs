using Unity.Game;
using UnityEngine;
using Zenject;

namespace Core.Application.Spells.Targeting
{
    public class FieldTargetingComponent : SpellTargetingComponent
    {
        [Inject] private GameController _gameController;

        private Vector2? _targetPosition;
        
        public override bool IsTargetingActive()
        {
            return !_targetPosition.HasValue;
        }

        public override void Activate()
        {
            _gameController.LocationSwitcher.CurrentLocation.ActivateRoadHighLight(true);
            _gameController.LocationSwitcher.CurrentLocation.OnRoadClick += OnRoadClick;
        }

        private void OnRoadClick(Vector2 position)
        {
            _targetPosition = position;
        }

        public override void Deactivate()
        {
            _gameController.LocationSwitcher.CurrentLocation.ActivateRoadHighLight(false);
            _gameController.LocationSwitcher.CurrentLocation.OnRoadClick -= OnRoadClick;
        }
    }
}
