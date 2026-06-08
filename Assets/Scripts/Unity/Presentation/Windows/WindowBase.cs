using System;
using Core.Application.Interfaces.Windows;
using UnityEngine;

namespace Unity.Presentation.Windows
{

	public abstract class WindowBase : MonoBehaviour, IWindow
	{

		public event Action<IWindow> OnShow;
		public event Action<IWindow> OnHide;
		
		protected virtual void Awake()
		{
			Hide();
		}

		public virtual void Initialize() { }
		
		public virtual void Show()
		{
			gameObject.SetActive(true);
			OnShow?.Invoke(this);
		}

		public virtual void Hide()
		{
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