using System;
using Core.Application.Interfaces.Windows;
using Unity.Presentation.Components;
using UnityEngine;

namespace Unity.Presentation.Windows
{

	public abstract class WindowBase : MonoBehaviour, IWindow
	{
		[SerializeField, HideInInspector]
		protected WindowsAnimatorController _animatorController;

		public event Action<IWindow> OnShow;
		public event Action<IWindow> OnHide;
		public event Action<IWindow> OnStartHide;
		
		protected virtual void OnValidate()
		{
			_animatorController = GetComponentInChildren<WindowsAnimatorController>();
		}

		public virtual void Initialize() { }
		
		public virtual void Show()
		{
			_animatorController?.Show();
			OnShow?.Invoke(this);
		}

		public virtual void Hide()
		{
			_animatorController?.Hide();
			OnStartHide?.Invoke(this);
			//Close();//TODO: вызывать клоус только после сокрытия окна
		}

		private void Close()
		{
			OnHide?.Invoke(this);
			Destroy(gameObject);
		}
		
		public GameObject GameObject => gameObject;

	}
}