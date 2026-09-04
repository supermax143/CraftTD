using UnityEngine;
using UnityEngine.UI;

namespace Unity.Presentation.Views.Spells
{
    public class StateSpriteSwitcher : MonoBehaviour
    {
        public enum State
        {
            None,
            Disabled,
            Enabled,
            Selected
        }
        
        [SerializeField]
        private Image _image;
        [SerializeField]
        private Sprite _disabledSprite;
        [SerializeField]
        private Sprite _enabledSprite;
        [SerializeField]
        private Sprite _selectedSprite;
        
        private  State _state = State.None;
        
        public void SetState(State state)
        {
            if (_state == state)
            {
                return;
            }
            _state = state;
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            switch (_state)
            {
                case State.Disabled:
                    _image.sprite = _disabledSprite;
                    break;
                case  State.Enabled:
                    _image.sprite = _enabledSprite;
                    break;
                case  State.Selected:
                    _image.sprite = _selectedSprite;
                    break;
                default:
                    _image.sprite = null;
                    break;
            }
        }
    }
}