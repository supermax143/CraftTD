using System;
using Core.Application.Interfaces.Windows;
using UnityEngine;

namespace Unity.Presentation.Windows
{

	public abstract class WindowBase : MonoBehaviour, IWindow
	{
		[SerializeField] private GameObject windowRoot;

		public event Action<IWindow> OnShow;
		public event Action<IWindow> OnHide;
		
		protected virtual void Awake()
		{
			if (windowRoot == null) windowRoot = gameObject;

			Hide();
		}

		public virtual void Show()
		{
			if (windowRoot != null)
				windowRoot.SetActive(true);
			else
				gameObject.SetActive(true);

			OnShow?.Invoke(this);
		}

		public virtual void Hide()
		{
			if (windowRoot != null)
				windowRoot.SetActive(false);
			else
				gameObject.SetActive(false);

			OnHide?.Invoke(this);
		}

		public void Close()
		{
			Destroy(gameObject);
		}
		
		public GameObject GameObject => gameObject;

	}
}