using System.Collections.Generic;
using UnityEngine;

namespace Unity.Presentation.Components.TabBar
{
    /// <summary>
    /// Контейнер для управления табами. Обеспечивает переключение между TabBarButton
    /// </summary>
    public class TabBar : MonoBehaviour
    {
        [SerializeField]
        private TabBarButton _defaultTab;

        private List<TabBarButton> _tabs = new List<TabBarButton>();
        private TabBarButton _currentTab;

        public TabBarButton CurrentTab => _currentTab;

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
            TabBarButton[] childTabs = GetComponentsInChildren<TabBarButton>();
            _tabs.AddRange(childTabs);
        }

        public void SelectTab(TabBarButton tab)
        {
            if (tab == null || !_tabs.Contains(tab)) return;

            if (_currentTab != null)
            {
                _currentTab.SetState(TabState.Idle);
            }

            tab.SetState(TabState.Selected);
            _currentTab = tab;
        }

        public void SelectTabByValue(string value)
        {
            TabBarButton tab = _tabs.Find(t => t.Value == value);
            SelectTab(tab);
        }

        public void SetTabDisabled(TabBarButton tab, bool disabled)
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
