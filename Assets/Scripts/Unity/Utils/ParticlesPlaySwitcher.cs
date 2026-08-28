using UnityEngine;

namespace Unity.Utils
{
    public class ParticlesPlaySwitcher : MonoBehaviour
    {
        
        [SerializeField, HideInInspector]
        private ParticleSystem[] _particleSystems;

        private void OnValidate()
        {
            _particleSystems = GetComponentsInChildren<ParticleSystem>(true);
        }

        [ContextMenu("Play")]
        public void Play()
        {
            foreach (var particleSystem in _particleSystems)
            {
                particleSystem.Play();
            }
        }
        
        [ContextMenu("Stop")]
        public void Stop()
        {
            foreach (var particleSystem in _particleSystems)
            {
                particleSystem.Stop();
            }
        }
    }
}