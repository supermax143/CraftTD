using CartoonFX;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity.Infrastructure.Effects.TextBubbleEffect
{
    [RequireComponent(typeof(CFXR_ParticleText))]
    public class TextBubbleVisualEffect : Popup
    {
        [SerializeField, HideInInspector]
        private CFXR_ParticleText _particleText;
        [SerializeField]
        private float _size = 1;
        [SerializeField]
        private float _rotationMin = -40;
        [SerializeField]
        private float _rotationMax = 40;
        
        private void OnValidate()
        {
            _particleText = GetComponent<CFXR_ParticleText>();
        }
        
        public void SetText(string text)
        {
            var rotation = Random.Range(_rotationMin, _rotationMax);
            _particleText.UpdateText(text, _size, null, null, null, null, rotation);
        }

    }
}