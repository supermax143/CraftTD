using System.Collections;
using UnityEngine;

namespace Unity.Infrastructure.Effects.AnimatedPopup
{
    [RequireComponent(typeof(PopupAnimatorController))]
    public class AnimatedPopup : Popup
    {
        [SerializeField, HideInInspector]
        private PopupAnimatorController _animatorController;
        
        private void OnValidate()
        {
            _animatorController = GetComponent<PopupAnimatorController>();
        }

        public override void Spawn()
        {
            base.Spawn();
            _animatorController.Show();
        }

        protected override IEnumerator WaitFinish()
        {
            yield return new WaitForSeconds(_time);
            _animatorController.Hide();
        }
        
        public void HideComplete()
        {
            DispatchComplete();
        }
    }
}