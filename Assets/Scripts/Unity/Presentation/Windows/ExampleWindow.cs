using System;
using System.Threading.Tasks;
using Core.Application.Interfaces.Windows;
using Unity.Infrastructure.Windows;
using Zenject;

namespace Unity.Presentation.Windows
{
	[Window(nameof(ExampleWindow))]
	public class ExampleWindow : WindowBase
	{
		[Inject] private IWindowsController _windowsManager;
		
		private void Start()
		{
			float randomScale = UnityEngine.Random.Range(.5f, 1f);
			transform.localScale = transform.localScale * randomScale;
		}

		public void ShowAnotherWindow()
		{
			_windowsManager.ShowWindow<ExampleWindow>((window) =>
			{
				window.Show();
			});
		}
	}
}