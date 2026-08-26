using System.Collections;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game.Spells.Execution
{
    public class ProjectileExecutionComponent : SpellExecutionComponent 
    {
        [SerializeField]
        private Vector2 _startDelta;
        [SerializeField]
        private float _flyTime;
        [SerializeField]
        private GameObject _projectile;
        [SerializeField]
        private GameObject _explosion;
        
        private Vector2 _startPosition;
        private Vector2 _targetPosition;
        
        public override void Execute(SpellController spell)
        {
            _projectile.SetActive(false);
            _explosion.SetActive(false);
            _targetPosition = spell.TargetingComponent.TargetPosition;
            _startPosition = _targetPosition - _startDelta;
            StartCoroutine(AnimateProjectile());
        }

        private IEnumerator AnimateProjectile()
        {
            _projectile.SetActive(true);
            var timer = new Timer();
            timer.Start(_flyTime);
            while (!timer.IsComplete)
            {
                transform.position = Vector3.Lerp(_startPosition, _targetPosition, timer.Progress);
                yield return null;
            }
            transform.position = _targetPosition;
            _explosion.SetActive(true);
        }
    }
}