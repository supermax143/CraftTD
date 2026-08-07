using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.Sound
{
    /// <summary>
    /// Менеджер для воспроизведения звуковых эффектов
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        [SerializeField]
        private float _randomPitchMin = 0.9f;
        
        [SerializeField]
        private float _randomPitchMax = 1.1f;
        
        [SerializeField]
        private AudioSource _audioSource;

        public void PlaySound(AudioClip clip, bool randomPitch = false)
        {
            if (clip == null || _audioSource == null)
            {
                return;
            }

            if (randomPitch)
            {
                _audioSource.pitch = Random.Range(_randomPitchMin, _randomPitchMax);
            }
            else
            {
                _audioSource.pitch = 1f;
            }

            _audioSource.PlayOneShot(clip);
        }
    }
}
