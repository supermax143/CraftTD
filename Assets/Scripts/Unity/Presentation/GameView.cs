using Core.Application.Interfaces.Windows;
using Unity.Presentation.Windows;
using UnityEngine;
using Zenject;

namespace Unity.Presentation
{
    public class GameView : MonoBehaviour
    {
        [Inject] private IWindowsController _windowsController;
        
        public void ShowExampleWindow()
        {
           _windowsController.ShowWindow<ExampleWindow>( window => window.Show());
           
        }
    }
}