using UnityEngine;

namespace Unity.Infrastructure.Windows
{
	internal interface IWindowsMemberHolder
	{
		void OnWindowRemoved(WindowsListMember member);
	}

	internal sealed class WindowsListMember : MonoBehaviour
	{
		private IWindowsMemberHolder _windowsController;

		public string WindowName { get; private set; }

		private void OnDestroy()
		{ 
			_windowsController.OnWindowRemoved(this);
		}

		internal void Initialize(IWindowsMemberHolder windowsController, string windowName)
		{
			_windowsController = windowsController;
			WindowName = windowName;
		}
	}
}