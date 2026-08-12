using System;
using Core.Application.Interfaces.Views;
using Unity.Presentation.Components;
using UnityEngine;

namespace Unity.Presentation.Views
{
	public abstract class ViewBase : MonoBehaviour, IView
	{
		[SerializeField, HideInInspector]
		protected ViewAnimatorController _animatorController;

		public event Action<IView> OnShow;
		public event Action<IView> OnHide;
		public event Action<IView> OnStartHide;
		
		protected virtual void OnValidate()
		{
			_animatorController = GetComponentInChildren<ViewAnimatorController>();
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
		}

		public void Close()
		{
			OnHide?.Invoke(this);
			Destroy(gameObject);
		}
		
		public GameObject GameObject => gameObject;
	}
}
