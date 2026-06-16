using Sirenix.OdinInspector;
using Unity.Game.Attributes.Specific;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    public abstract class TargetSearchComponentBase : GameComponent
    {
        public const float DETECTION_INTERVAL = .3f;

        [Inject] private IGameController _gameController;
        [Inject] private GameStats _gameStats;

        [SerializeField]
        private AttackRangeAttribute _attackRange;

        public Transform SearchTransform => transform;
        public float DetectionRange => _gameStats.DetectionRange;
        public float AttackRange => _attackRange.ValueModified;

        private Faction _faction;
        protected Faction _opponentFaction;

        public void SetFaction(Faction faction, Faction opponentFaction)
        {
            _faction = faction;
            _opponentFaction = opponentFaction;
        }

        public abstract bool TryGetClosestTarget(out AttackTargetBase target);

        public bool TryGetTargetTower(out AttackTargetBase target)
            => _gameController.TryGetOpponentTower(_opponentFaction, out target);

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
        }

        protected abstract void DrawCircle(Vector3 center, float radius);
    }
}
