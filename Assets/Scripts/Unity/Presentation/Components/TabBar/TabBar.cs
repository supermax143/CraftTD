using System.Collections.Generic;
using UnityEngine;

namespace Unity.Presentation.Components.TabBar
{
    /// <summary>
    /// Контейнер для управления табами. Обеспечивает переключение между TabBarButton
    /// </summary>
    public abstract class TabBar<T> : MonoBehaviour
    {
        [SerializeField]
        private TabBarButton<T> _defaultTab;

        private List<TabBarButton<T>> _tabs = new List<TabBarButton<T>>();
        private TabBarButton<T> _currentTab;

        public TabBarButton<T> CurrentTab => _currentTab;

        private void Awake()
        {
            CollectTabs();
        }

        private void Start()
        {
            if (_defaultTab != null)
            {
                SelectTab(_defaultTab);
            }
        }

        private void CollectTabs()
        {
            _tabs.Clear();
            TabBarButton<T>[] childTabs = GetComponentsInChildren<TabBarButton<T>>();
            _tabs.AddRange(childTabs);
        }

        public virtual void SelectTab(TabBarButton<T> tab)
        {
            if (tab == null || !_tabs.Contains(tab)) return;

            if (_currentTab != null)
            {
                _currentTab.SetState(TabState.Idle);
            }

            tab.SetState(TabState.Selected);
            _currentTab = tab;
        }

        public void SelectTabByValue(T value)
        {
            TabBarButton<T> tab = _tabs.Find(t => EqualityComparer<T>.Default.Equals(t.Value, value));
            SelectTab(tab);
        }

        public void SetTabDisabled(TabBarButton<T> tab, bool disabled)
        {
            if (tab == null || !_tabs.Contains(tab)) return;

            if (disabled)
            {
                if (tab == _currentTab)
                {
                    _currentTab = null;
                }
                tab.SetState(TabState.Disabled);
            }
            else
            {
                tab.SetState(TabState.Idle);
            }
        }
    }
}
