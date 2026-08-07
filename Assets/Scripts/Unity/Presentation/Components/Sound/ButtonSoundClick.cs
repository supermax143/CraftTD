using System;
using Unity.Infrastructure.Sound;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.Components.Sound
{
    /// <summary>
    /// Компонент для воспроизведения звука при клике на кнопку
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ButtonSoundClick : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _clickSound;
        [SerializeField, HideInInspector]
        private Button _button;
        

        [Inject]
        private SoundManager _soundManager;

        private void OnValidate()
        {
            _button = GetComponent<Button>();
        }

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _soundManager.PlaySound(_clickSound, true);
        }
    }
}
