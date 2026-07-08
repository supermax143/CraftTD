using System.Collections;
using Unity.Presentation.Components;
using UnityEngine;
using Utils.ColorEffects;

namespace Unity.Game
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private TintController _tintController;
        [SerializeField, HideInInspector]
        private UnitAnimatorController _unitAnimatorController;
        [SerializeField, HideInInspector]
        private FieldObjectEffectsController _effectsController;
        
        
        private HealthComponent _healthComponent;
        private Color _color;
        private Transform _rootTransform;
        
        
        private void OnValidate()
        {
            _tintController = GetComponentInChildren<TintController>();
            _unitAnimatorController = GetComponentInChildren<UnitAnimatorController>();
            _effectsController = GetComponentInChildren<FieldObjectEffectsController>();
        }

        public void Initialize(HealthComponent healthComponent, Transform rootTransform)
        {
            _rootTransform = rootTransform;
            _healthComponent = healthComponent;
            _healthComponent.OnDamage += OnDamage;
        }
        

        private void OnDamage(int damage)
        {
            StartCoroutine(DamageAnimation());
        }

        private IEnumerator DamageAnimation()
        {
            _tintController.SetTintColor(Color.white);
            yield return _effectsController.ShowHitEffect(.2f);
            _tintController.SetTintColor(_color);
        }

        public void SetColor(Color color)
        {
            _color = color;
            _tintController.SetTintColor(color);
        }

        private void OnDestroy()
        {
            if (_healthComponent != null)
            {
                _healthComponent.OnDamage -= OnDamage;
            }
        }
        
        public void StartWalking()
        {
            _unitAnimatorController.PlayWalk();
        }
        
        public void StartAttacking()
        {
            _unitAnimatorController.PlayAttack();
        }
        
        public void StartIdle()
        {
            _unitAnimatorController.PlayIdle();
        }
        
        [ContextMenu("Die")]
        public void StartDie()
        {
            _unitAnimatorController.PlayDie();
            StartCoroutine(DeathAnimation());
        }
        
        private IEnumerator DeathAnimation()
        {
            yield return new WaitForSeconds(1);
            yield return _effectsController.ShowDissolveEffect(.7f);
        }
        

        public void UpdateSortingByPosition()
        {
            var pos = _rootTransform.position;
            pos.z = pos.y * 0.001f;
            _rootTransform.position = pos;
        }
        
        public void SetRandomFrame()
        {
            _unitAnimatorController.SetRandomFrame();
        }

    }
}