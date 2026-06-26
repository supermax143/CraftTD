using System;
using Unity.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity.Presentation.Components
{
    public class UnitAnimatorController : AnimatorControllerBase
    {
        
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
        }
        
        private static class Triggers
        {
            public static readonly int Idle = Animator.StringToHash(nameof(Idle));
            public static readonly int Walk = Animator.StringToHash(nameof(Walk));
            public static readonly int Attack = Animator.StringToHash(nameof(Attack));
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
        
        
        public bool IsIdleState() => IsPlayingState(States.Idle, Layers.Base);
        public bool IsWalkState() => IsPlayingState(States.Walk, Layers.Base);
        public bool IsAttackState() => IsPlayingState(States.Attack, Layers.Base);
    }
}