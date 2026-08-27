using Environments.Common.Scripts;
using Unity.Game;
using UnityEngine;
using Zenject;

namespace Core.Application.Spells.Targeting
{
    public class UnitTargetingComponent : SpellTargetingComponent
    {
        [SerializeField]
        private Faction _faction;
        
        [Inject] private IGameController _gameController;
        
        public override bool IsTargetingActive()
        {
            return TargetPosition == default;
        }

        public override void Activate()
        {
            var team = _gameController.GetTeam(_faction);
            foreach (var unit in team.Units)
            {
                unit.OnClick += HandleClick;
                unit.View.ShowHideSelection(true);
            }
        }

        private void HandleClick(ITouchTarget target, Vector2 touchPosition)
        {
            TargetPosition = Camera.main.ScreenToWorldPoint(touchPosition);
            Target = target;
        }

        public override void Deactivate()
        {
            var team = _gameController.GetTeam(_faction);
            foreach (var unit in team.Units)
            {
                unit.OnClick -= HandleClick;
                unit.View.ShowHideSelection(false);
            }
        }
    }
}