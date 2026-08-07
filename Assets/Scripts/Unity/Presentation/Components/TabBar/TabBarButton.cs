using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.Presentation.Components.TabBar
{
    /// <summary>
    /// Кнопка таба с тремя состояниями: Idle, Selected, Disabled
    /// </summary>
    public abstract class TabBarButton<T> : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private T _value;

        private TabBarButtonAnimatorController _animatorController;
        private TabBar<T> _tabBar;
        private TabState _state = TabState.Idle;

        public T Value => _value;
        public TabState State => _state;

        private void Awake()
        {
            _animatorController = GetComponent<TabBarButtonAnimatorController>();
            _tabBar = GetComponentInParent<TabBar<T>>();
        }

        public void SetState(TabState state)
        {
            _state = state;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (_animatorController == null) return;

            switch (_state)
            {
                case TabState.Idle:
                    _animatorController.SetSelectedState(false);
                    _animatorController.SetDisabledState(false);
                    break;
                case TabState.Selected:
                    _animatorController.SetSelectedState(true);
                    _animatorController.SetDisabledState(false);
                    break;
                case TabState.Disabled:
                    _animatorController.SetSelectedState(false);
                    _animatorController.SetDisabledState(true);
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
