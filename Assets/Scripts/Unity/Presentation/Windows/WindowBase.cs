using System;
using Core.Application.Interfaces.Windows;
using UnityEngine;

namespace Unity.Presentation.Windows
{

	public abstract class WindowBase : MonoBehaviour, IWindow
	{

		public event Action<IWindow> OnShow;
		public event Action<IWindow> OnHide;
		public event Action<IWindow> OnStartHide;
		
		protected virtual void Awake()
		{
			gameObject.SetActive(false);
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
			OnStartHide?.Invoke(this);
			Close();//TODO: вызывать клоус только после сокрытия окна
		}

		private void Close()
		{
			OnHide?.Invoke(this);
			Destroy(gameObject);
		}
		
		public GameObject GameObject => gameObject;

	}
}