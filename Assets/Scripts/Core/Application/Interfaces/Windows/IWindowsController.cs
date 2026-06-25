using System;
using Cysharp.Threading.Tasks;

namespace Core.Application.Interfaces.Windows
{
   public interface IWindowsController
   {
      UniTask<TWindow> ShowWindow<TWindow>() where TWindow : class, IWindow;
      void ShowWindow<TWindow>(Action<IWindow> handler) where TWindow : class, IWindow;
   }
}