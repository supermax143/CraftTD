using Core.Application.Spells.Activation;
using Environments.Common.Scripts;
using Unity.Game;
using UnityEngine;
using Zenject;

namespace Core.Application.Spells.Targeting
{
    public class UnitTargetingActivationComponent : SpellActivationComponent
    {
        [SerializeField]
        private Faction _faction;
        
        [Inject] private IGameController _gameController;
        
        
        public override void StartActivationCheck()
        {
            IsActivationCheckActive = true;
            var team = _gameController.GetTeam(_faction);
            foreach (var unit in team.Units)
            {
                unit.OnClick += HandleClick;
                unit.View.ShowHideSelection(true);
            }
        }

        private void HandleClick(ITouchTarget target, Vector2 touchPosition)
        {
            IsActivationCheckActive = false;
            TargetPosition = Camera.main.ScreenToWorldPoint(touchPosition);
            Target = target;
        }

        public override void StopActivationCheck()
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