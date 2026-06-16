using System;
using UnityEngine;

namespace Unity.Presentation.Components
{
    public class UnitAnimator : MonoBehaviour
    {
        
        private Animator _animator;

        private int _currentTrigger;

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
        
        
        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void PlayAttack()
        {
            if (IsAttackState())
            {
                return;
            }
            PlayAnimation(Triggers.Attack);
        }
        
        public void PlayIdle()
        {
            if (IsIdleState())
            {
                return;
            }
            PlayAnimation(Triggers.Idle);
        }
        
        public void PlayWalk()
        {
            if (IsWalkState())
            {
                return;
            }
            PlayAnimation(Triggers.Walk);
        }
        
        
        
        public bool IsIdleState() => IsPlayingState(States.Idle, Layers.Base);
        public bool IsWalkState() => IsPlayingState(States.Walk, Layers.Base);
        public bool IsAttackState() => IsPlayingState(States.Attack, Layers.Base);
        
        protected bool IsPlayingState(int shortNameHash, int layerIndex)
        {
            if (_animator == null)
            {
                return false;
            }
            var state = _animator.GetCurrentAnimatorStateInfo(layerIndex);
            return shortNameHash == state.shortNameHash;
        }
        
        private void PlayAnimation(int trigger)
        {
            if (_animator == null)
            {
                return;
            }
            
            if (_currentTrigger != 0)
            {
                _animator.ResetTrigger(_currentTrigger);
            }

            _animator.SetTrigger(trigger);
            _currentTrigger = trigger;
        }
        
    }
}