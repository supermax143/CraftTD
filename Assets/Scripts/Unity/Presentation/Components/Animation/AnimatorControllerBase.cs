using UnityEngine;

namespace Unity.Presentation.Components
{
    public class AnimatorControllerBase : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        protected Animator _animator;

        private int _currentTrigger;

        protected virtual void OnValidate()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void SetBool(int id,bool value)
        {
            _animator.SetBool(id, value);
        }
        
        protected void PlayAnimation(int trigger)
        {
            if (_animator == null)
            {
                return;
            }

            if (_currentTrigger == trigger)
            {
                return;
            }
            
            if (_currentTrigger != 0)
            {
                _animator.ResetTrigger((int)_currentTrigger);
            }

            _animator.SetTrigger(trigger);
            _currentTrigger = trigger;
        }

        protected bool IsPlayingState(int shortNameHash, int layerIndex)
        {
            if (_animator == null)
            {
                return false;
            }
            var state = _animator.GetCurrentAnimatorStateInfo(layerIndex);
            return shortNameHash == state.shortNameHash;
        }

        public void SetRandomFrame(int layerIndex = 0)
        {
            if (_animator == null)
            {
                return;
            }

            var stateInfo = _animator.GetCurrentAnimatorStateInfo(layerIndex);
            var randomTime = Random.Range(0f, stateInfo.length);
            _animator.PlayInFixedTime(stateInfo.shortNameHash, layerIndex, randomTime);
        }
    }
}