using System;
using Core.Application.Interfaces.Views;
using Cysharp.Threading.Tasks;
using Unity.Infrastructure.ResourceManager;
using Unity.Presentation.Views;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.Views
{
	public class ViewsController : MonoBehaviour, IViewsController
	{
		[Inject] private readonly DiContainer _diContainer;

		private ViewBase _currentView;
		private string _loadingView;

		public event Action<string> OnViewStartLoading;
		public event Action OnActiveViewChanged;
		public event Action OnViewLoadComplete;
		public event Action<string> OnViewClosed;

		private void OnViewRemoved(ViewBase view)
		{
			if (_currentView == view)
			{
				_currentView = null;
				AddressableExtention.ReleaseTag(GetViewUnloadTag(view.name));
				OnActiveViewChanged?.Invoke();
				OnViewClosed?.Invoke(view.name);
			}
		}

		public UniTask<TView> ShowView<TView>() where TView : class, IView
		{
			throw new NotImplementedException();
		}

		public void ShowView<TView>(Action<TView> handler) where TView : class, IView
		{
			throw new NotImplementedException();
		}
		
		
		/*public async void ShowView<TView>(Transform parent, Action<TView> handler) where TView : class, IView
		{
			ShowViewInternal<TView>(parent, handler).Forget();
		}*/
		
		private async UniTask ShowViewInternal<TView>(Transform parent, Action<TView> handler) where TView : class, IView
		{
			var view = await ShowView<TView>(parent);
			handler?.Invoke(view);
		}
		
		/*public async UniTask<TView> ShowView<TView>(Transform parent) where TView : class, IView
		{

			var view = "dummy view";//TODO: сделать красиво
			var viewType = typeof(TView);
			

			try
			{
				_loadingView = view;
				OnViewStartLoading?.Invoke(view);

				var viewPrefab = await AddressableExtention.Load<GameObject>(view, GetViewUnloadTag(view));
				if (viewPrefab == null)
				{
					Debug.LogError($"Failed to load view prefab: {view}");
					return null;
				}

				return InitializeInstance<TView>(viewPrefab, view, parent);
			}
			catch (Exception ex)
			{
				Debug.LogError($"Exception while loading view {view}: {ex}");
				return null;
			}
			finally
			{
				_loadingView = null;
			}
		}*/

		private TView InitializeInstance<TView>(GameObject viewPrefab, string viewName, Transform parent) where TView : class, IView
		{
			if (_currentView != null)
			{
				Destroy(_currentView.gameObject);
			}

			var view = _diContainer.InstantiatePrefabForComponent<TView>(viewPrefab, parent);

			_currentView = view as ViewBase;
			_loadingView = null;

			var rectTransform = view.GameObject.GetComponent<RectTransform>();
			rectTransform.SetParent(parent, false);

			(view as ViewBase).Initialize();
			OnViewLoadComplete?.Invoke();
			OnActiveViewChanged?.Invoke();

			return view;
		}

		private string GetViewUnloadTag(string viewName) => $"{viewName}-{GetHashCode()}";


		public UniTask<TView> ShowView<TView>(Transform parent) where TView : class, IView
		{
			throw new NotImplementedException();
		}

		public void ShowView<TView>(Transform parent, Action<TView> handler) where TView : class, IView
		{
			throw new NotImplementedException();
		}
	}
}
