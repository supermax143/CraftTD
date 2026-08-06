using System;
using Unity.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity.Presentation.Components
{
    public class UnitAnimatorController : AnimatorControllerBase
    {
        [SerializeField]
        private AnimationClip[] _dieClips;

        [SerializeField, HideInInspector]
        private UnitAnimationEvents _animationEvents;

        private static class Layers
        {
            public static readonly int Base = 0;
        }

        private static class States
        {
            public static readonly int Idle = Animator.StringToHash(nameof(Idle));
            public static readonly int Walk = Animator.StringToHash(nameof(Walk));
            public static readonly int Attack = Animator.StringToHash(nameof(Attack));
            public static readonly int Die = Animator.StringToHash(nameof(Die));
        }
        
        private static class Triggers
        {
            public static readonly int Idle = Animator.StringToHash(nameof(Idle));
            public static readonly int Walk = Animator.StringToHash(nameof(Walk));
            public static readonly int Attack = Animator.StringToHash(nameof(Attack));
            public static readonly int Die = Animator.StringToHash(nameof(Die));
        }
        

        protected void OnValidate()
        {
            
            base.OnValidate();
            if (_animator != null && !_animator.TryGetComponent(out _animationEvents))
            {
                _animationEvents = _animator.gameObject.AddComponent<UnitAnimationEvents>();
            }
        }

        public void PlayAttack()
        {
            PlayAnimation(Triggers.Attack);
        }
        
        public void PlayIdle()
        {
            PlayAnimation(Triggers.Idle);
        }
        
        public void PlayWalk()
        {
            PlayAnimation(Triggers.Walk);
        }
        
        public void PlayDie()
        {
            if (_dieClips != null && _dieClips.Length > 0)
            {
                var randomClip = _dieClips[Random.Range(0, _dieClips.Length)];
                var overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
                overrideController["Die"] = randomClip;
                _animator.runtimeAnimatorController = overrideController;
            }
            PlayAnimation(Triggers.Die);
        }
        
        public void PauseAnimation()
        {
            _animator.speed = 0;
        }
       
        public void UnpauseAnimation()
        {
            _animator.speed = 1;
        }
        
        public bool IsIdleState() => IsPlayingState(States.Idle, Layers.Base);
        public bool IsWalkState() => IsPlayingState(States.Walk, Layers.Base);
        public bool IsAttackState() => IsPlayingState(States.Attack, Layers.Base);

    }
}