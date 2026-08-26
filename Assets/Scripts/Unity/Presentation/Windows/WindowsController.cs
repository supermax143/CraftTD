using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application.Interfaces.Windows;
using Cysharp.Threading.Tasks;
using Environments.Common.Scripts;
using Unity.Infrastructure.ResourceManager;
using Unity.Presentation.Components;
using Unity.Presentation.Windows;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.Windows
{
	
	internal class WindowsController : MonoBehaviour,
        IWindowsMemberHolder, IWindowsController, ITouchBlocker
    {

        [SerializeField] 
        private Transform _windowsParent;
        [SerializeField]
        private OverlayComponent _background;
        
        [Inject] private readonly DiContainer _diContainer;
        private readonly List<string> _loadingWindows = new();


        private readonly List<WindowsListMember> _windowsList = new();
        private ITouchController _touchController;

        public event Action<string> OnWindowStartLoading;
        public event Action OnActiveWindowChanged;
        public event Action OnWindowLoadComplete;
        public event Action<string> OnAnyWindowClosed;
        public event Action OnLastWindowClosed;


        void IWindowsMemberHolder.OnWindowRemoved(WindowsListMember member)
        {
            if (!_windowsList.Any())
            {
                Debug.LogError("Calling OnWindowRemoved but windowsList is empty");
                return;
            }

            var isLastMember = _windowsList.Last() == member;

            _windowsList.Remove(member);
            AddressableExtention.ReleaseTag(GetWindowUnloadTag(member.WindowName));

            var isWindowsListEmpty = !_windowsList.Any();
            if (isLastMember)
            {
                OnActiveWindowChanged?.Invoke();
            }

            if (isWindowsListEmpty && !_loadingWindows.Any())
            {
                _background.Hide();
                OnLastWindowClosed?.Invoke();
                _touchController.Unblock(this);
            }
            UpdateBackgroundIndex();
            
            OnAnyWindowClosed?.Invoke(member.WindowName);
        }
        
        public void ShowWindow<TWindow>(Action<TWindow> handler) where TWindow : class, IWindow
        {
            ShowWindowInternal<TWindow>(handler).Forget();
        }

        public void SetTouchController(ITouchController touchController)
        {
            _touchController = touchController;
        }

        private async UniTask ShowWindowInternal<TWindow>(Action<TWindow> handler) where TWindow : class, IWindow
        {
            var window = await ShowWindow<TWindow>();
            handler?.Invoke(window);
        }
        

        public async UniTask<TWindow> ShowWindow<TWindow>() where TWindow : class, IWindow
        {
            var windowType =  typeof(TWindow);
            if (!WindowAttribute.TryGetName(windowType, out var windowName))
            {
                Debug.LogError($"Can't find Window attribute on Type {windowType.Name}");
                return null;
            }
            try
            {
                
                _loadingWindows.Add(windowName);
                OnWindowStartLoading?.Invoke(windowName);

                var windowPrefab = await AddressableExtention.Load<GameObject>(windowName, GetWindowUnloadTag(windowName));
                if (windowPrefab == null)
                {
                    Debug.LogError($"Failed to load window prefab: {windowName}");
                    return null;
                }
                
                
                return InitializeInstance<TWindow>(windowPrefab, windowName);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception while loading window {windowName}: {ex}");
                return null;
            }
            finally
            {
                _loadingWindows.Remove(windowName);
            }
            
        }

        private TWindow InitializeInstance<TWindow>(GameObject windowPrefab, string windowName)where TWindow : class, IWindow
        {
            var window = _diContainer.InstantiatePrefabForComponent<TWindow>(windowPrefab, _windowsParent);
            var windowsListMember = window.GameObject.AddComponent<WindowsListMember>();
            windowsListMember.Initialize(this, windowName);

            _background.Show();
            _touchController.Block(this);
            
            _loadingWindows.Remove(windowsListMember.WindowName);

            ShowWindowInternal(windowsListMember);

            _windowsList.Add(windowsListMember);
            UpdateBackgroundIndex();

            (window as WindowBase).Initialize();
            OnWindowLoadComplete?.Invoke();

            return window;
        }

        private void UpdateBackgroundIndex()
        {
            if (_windowsList.Count == 0)
            {
                return;
            }
            _background.transform.SetSiblingIndex(_windowsList.Count - 1);
        }

        private void ShowWindowInternal(WindowsListMember member)
        {
            OnActiveWindowChanged?.Invoke();
            
            var rectTransform = member.GetComponent<RectTransform>();
            rectTransform.SetParent(_windowsParent, false);
        }

        private string GetWindowUnloadTag(string windowName) => $"{windowName}-{GetHashCode()}";
    }
}