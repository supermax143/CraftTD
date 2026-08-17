using Unity.Game.Attributes.Specific;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    public abstract class TargetSearchComponentBase : GameComponent
    {
        public const float DETECTION_INTERVAL = .1f;

        [Inject] private IGameController _gameController;
        [Inject] private GameStats _gameStats;

        [SerializeField]
        private AttackRangeAttribute _attackRange;

        private MoveComponentBase _moveComponent;

        public Transform SearchTransform => transform;
        public float DetectionRange => _gameStats.DetectionRange;
        public float AttackRange => _attackRange.BaseValueModified;

        private Faction _faction;
        protected Faction _opponentFaction;

        public void SetFaction(Faction faction, Faction opponentFaction)
        {
            _faction = faction;
            _opponentFaction = opponentFaction;
            
        }

        public void Initialize(MoveComponentBase moveComponent)
        {
            _moveComponent = moveComponent;
        }
        

        public abstract bool TryGetClosestTarget(out AttackTargetBase target);

        public bool TryGetTargetTower(out AttackTargetBase target)
            => _gameController.TryGetTower(_opponentFaction, out target);

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            Gizmos.color = Color.blue;
            DrawCircle(transform.position, DetectionRange);
            Gizmos.color = Color.red;
            DrawCircle(transform.position, AttackRange);

            if (_moveComponent != null && _moveComponent.IsMoving)
            {
                Gizmos.color = Color.blue;
                if (!_moveComponent.TryTargetPosition(out var pos))
                {
                    return;
                }
                Gizmos.DrawLine(transform.position, pos);
            }
        }

        protected abstract void DrawCircle(Vector3 center, float radius);

    }
}
