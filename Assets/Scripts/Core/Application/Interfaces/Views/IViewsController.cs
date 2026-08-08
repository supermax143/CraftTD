using System;
using Cysharp.Threading.Tasks;
using log4net.Util;

namespace Core.Application.Interfaces.Views
{
   public interface IViewsController
   {
      UniTask<TView> ShowView<TView>() where TView : class, IView;
      void ShowView<TView>(Action<TView> handler) where TView : class, IView;
   }
}
