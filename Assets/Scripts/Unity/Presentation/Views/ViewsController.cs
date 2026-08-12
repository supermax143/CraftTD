using System;
using Core.Application.Interfaces.Views;
using Cysharp.Threading.Tasks;
using Unity.Infrastructure.ResourceManager;
using Unity.Presentation.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Unity.Infrastructure.Views
{
	public class ViewsController : MonoBehaviour, IViewsController
	{
		[SerializeField]
		private Transform _viewsContainer;
		
		[Inject] private readonly DiContainer _diContainer;

		private ViewInfo _currentView;
		private ViewInfo _prevView;
		public event Action OnActiveViewChanged;
		public event Action OnViewLoadComplete;
		public event Action<string> OnViewClosed;

		public bool IsInProgress => _prevView != null;
		
		public bool TryGetCurrentView(out IView view)
		{
			view = _currentView?.View;
			return view != null;
		}
		
		
		private void OnViewRemoved(IView view)
		{
			if (_prevView.View != view)
			{
				Debug.LogError("wrong previous view");
				return;
			}
			
			AddressableExtention.ReleaseTag(GetViewUnloadTag(_prevView.Id));
			OnViewClosed?.Invoke(_prevView.Id);
			_prevView.View.OnHide -= OnViewRemoved;
			_prevView = null;
			OnActiveViewChanged?.Invoke();
		}

		
		public void ShowView<TView>(AssetReference viewAsset, Action<TView> handler) where TView : class, IView
		{
			ShowViewInternal<TView>(viewAsset, handler).Forget();
		}
		
		private async UniTask ShowViewInternal<TView>(AssetReference viewAsset, Action<TView> handler) where TView : class, IView
		{
			var view = await ShowView<TView>(viewAsset);
			handler?.Invoke(view);
		}
		
		public async UniTask<TView> ShowView<TView>(AssetReference viewAsset) where TView : class, IView
		{
			if (IsInProgress)
			{
				Debug.Log($"View  is already in progress");
				return null;
			}
			
			
			try
			{
				if (_currentView != null)
				{
					_prevView = _currentView;
					_prevView.View.Hide();	
					_currentView = null;
				}
				
				/*while (_currentView != null)
				{
					await UniTask.Yield();
				}*/

				if (viewAsset == null || string.IsNullOrEmpty(viewAsset.AssetGUID))
				{
					return  null;
				}
				
				var viewPrefab = await viewAsset.LoadAssetReference<GameObject>(viewAsset.AssetGUID);
				if (viewPrefab == null)
				{
					Debug.LogError($"Failed to load view prefab: {viewAsset.AssetGUID}");
					return null;
				}

				
				return InitializeInstance<TView>(viewPrefab, viewAsset.AssetGUID);
			}
			catch (Exception ex)
			{
				Debug.LogError($"Exception while loading view {viewAsset.AssetGUID}: {ex}");
				return null;
			}
			
		}

		private TView InitializeInstance<TView>(GameObject viewPrefab, string viewId) where TView : class, IView
		{
			var view = _diContainer.InstantiatePrefabForComponent<TView>(viewPrefab, _viewsContainer);

			_currentView = new ViewInfo(viewId, view);

			var rectTransform = view.GameObject.GetComponent<RectTransform>();
			rectTransform.SetParent(_viewsContainer, false);
			view.OnHide += OnViewRemoved;
			(view as ViewBase).Initialize();
			OnViewLoadComplete?.Invoke();
			OnActiveViewChanged?.Invoke();

			return view;
		}

		private string GetViewUnloadTag(string viewName) => $"{viewName}-{GetHashCode()}";
	
		public class ViewInfo
		{
			public string Id { get; }
			public IView View { get; }

			public ViewInfo(string id, IView view)
			{
				Id = id;
				View = view;
			}
		}
		
	}
}
