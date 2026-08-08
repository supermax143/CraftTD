using System;
using UnityEngine;

namespace Core.Application.Interfaces.Views
{
	public interface IView
	{
		GameObject GameObject { get; }
		void Show();
		void Hide();
		event Action<IView> OnShow;
		event Action<IView> OnHide;
	}
}
