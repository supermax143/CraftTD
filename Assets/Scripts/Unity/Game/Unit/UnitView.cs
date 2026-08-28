using System.Collections;
using DG.Tweening;
using Unity.Presentation.Components;
using Unity.Utils.Time;
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
        [SerializeField]
        private Transform _effectsContainerBottom;
        [SerializeField]
        private Transform _effectsContainerMiddle;
        [SerializeField]
        private Transform _effectsContainerTop;
        
        private HealthComponent _healthComponent;
        private Color _color;
        private Transform _rootTransform;

        public Transform EffectsContainerBottom => _effectsContainerBottom;
        public Transform EffectsContainerMiddle => _effectsContainerMiddle;
        public Transform EffectsContainerTop => _effectsContainerTop;


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

        public void ShowHideSelection(bool show)
        {
            /*var color = show ? Color.yellow : Color.white;
            foreach (var renderer in _effectsController.Renderers)
            {
                renderer.color = color;
            }

            _tintController.SetTintColor(show?color:_color);*/
            StartCoroutine(_effectsController.SetSpellSelectionEffect(.2f, show ? 1 : 0));
            var color = show ? Color.yellow : Color.white;
            _tintController.SetTintColor(show?color:_color);
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

        public void Pause()
        {
            _unitAnimatorController.PauseAnimation();
        }
        
        public void Unpause()
        {
            _unitAnimatorController.UnpauseAnimation();
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
        
        public void Hide(float time, bool inversed)
        {
            StartCoroutine(_effectsController.ShowHorizontalDissolveEffect(time, 1, inversed));
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

        public void SetScale(float scale, float time)
        {
            StartCoroutine(AnimateScale(scale, time));
        }
        
        private IEnumerator AnimateScale(float scale, float time)
        {
            var curScale = transform.localScale;
            var targetScale = curScale * scale;
            var timer = new Timer();
            timer.Start(time);
            while (!timer.IsComplete)
            {
                transform.localScale = Vector3.Lerp(curScale, targetScale, timer.Progress);
                yield return null;
            }
            transform.localScale = targetScale;
            
        }
    }
}