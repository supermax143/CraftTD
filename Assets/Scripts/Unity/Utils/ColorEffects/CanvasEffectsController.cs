using System.Collections;
using Unity.Utils.Time;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.ColorEffects
{
    /// <summary>
    /// Контроллер эффектов для Canvas элементов (Image)
    /// Инстанцирует материал для каждого эффекта и передает его в Image
    /// </summary>
    public class CanvasEffectsController : MonoBehaviour
    {
        [SerializeField]
        private Material _effectsMaterial;

        [SerializeField, HideInInspector]
        private MaskableGraphic[] _images;

        private static class ShaderProperties
        {
            public static readonly int _Blink = Shader.PropertyToID(nameof(_Blink));
            public static readonly int _Grayscale = Shader.PropertyToID(nameof(_Grayscale));
            public static readonly int _GrayscaleAmount = Shader.PropertyToID(nameof(_GrayscaleAmount));
        }

        private readonly Timer _blinkAnimationTimer = new(TimeType.Scaled);
        private readonly Timer _grayscaleAnimationTimer = new(TimeType.Scaled);

        private Material _currentMaterial;

        private void OnValidate()
        {
            _images = GetComponentsInChildren<MaskableGraphic>();
        }

        /*private void UpdateMaterial()
        {
            if (_currentMaterial != null)
            {
                return;
            }
            _currentMaterial = new Material(_effectsMaterial);
            ApplyMaterialToImages(_currentMaterial);
        }*/

        private Material GetMaterial()
        {
            if (_currentMaterial == null)
            {
                _currentMaterial = new Material(_effectsMaterial);
                ApplyMaterialToImages(_currentMaterial);
            }
            return _currentMaterial;
        }
        
        public void StartBlink()
        {
            GetMaterial().SetFloat(ShaderProperties._Blink, 1);
        }
        
        public void StopBlink()
        {
            GetMaterial().SetFloat(ShaderProperties._Blink, 0);
        }
        
        public void ShowBlink(float time)
        {
            StartCoroutine(AnimateBlink(time));
        }
        
        public IEnumerator AnimateBlink(float time)
        {
            var material = GetMaterial();
            material.SetFloat(ShaderProperties._Blink, 1);
            yield return new WaitForSeconds(time);
            material.SetFloat(ShaderProperties._Blink, 0);
        }

        public void ShowGrayscale(float time, float value)
        {
            StartCoroutine(AnimateGrayscale(time, value));
        }

        public IEnumerator AnimateGrayscale(float time, float value)
        {
            var material = GetMaterial();

            var startValue = value == 1? 0 : 1;
            _grayscaleAnimationTimer.Start(time);
            material.SetFloat(ShaderProperties._Grayscale, 1);
            while (!_grayscaleAnimationTimer.IsComplete)
            {
                material.SetFloat(ShaderProperties._GrayscaleAmount, Mathf.Lerp(startValue, value, _grayscaleAnimationTimer.Progress));
                yield return null;
            }
            material.SetFloat(ShaderProperties._GrayscaleAmount, value);
            if (value == 0)
            {
                material.SetFloat(ShaderProperties._Grayscale, 0);
            }
            
        }

        private void ApplyMaterialToImages(Material material)
        {
            foreach (var image in _images)
            {
                if (image != null)
                {
                    image.material = material;
                }
            }
        }

        public void ResetImagesMaterial()
        {
            foreach (var image in _images)
            {
                if (image != null)
                {
                    image.material = null;
                }
            }
        }
    }
}
