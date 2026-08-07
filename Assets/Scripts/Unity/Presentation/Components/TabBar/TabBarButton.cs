using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unity.Presentation.Components.TabBar
{
    /// <summary>
    /// Кнопка таба с тремя состояниями: Idle, Selected, Disabled
    /// </summary>
    public class TabBarButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private string _value;

        [SerializeField]
        private Color _idleColor = Color.white;

        [SerializeField]
        private Color _selectedColor = Color.yellow;

        [SerializeField]
        private Color _disabledColor = Color.gray;

        private Image _image;
        private TabBar _tabBar;
        private TabState _state = TabState.Idle;

        public string Value => _value;
        public TabState State => _state;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _tabBar = GetComponentInParent<TabBar>();
        }

        public void SetState(TabState state)
        {
            _state = state;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (_image == null) return;

            switch (_state)
            {
                case TabState.Idle:
                    _image.color = _idleColor;
                    break;
                case TabState.Selected:
                    _image.color = _selectedColor;
                    break;
                case TabState.Disabled:
                    _image.color = _disabledColor;
                    break;
            }
        }

        public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (_state == TabState.Disabled) return;
            _tabBar?.SelectTab(this);
        }
    }

    public enum TabState
    {
        Idle,
        Selected,
        Disabled
    }
}
