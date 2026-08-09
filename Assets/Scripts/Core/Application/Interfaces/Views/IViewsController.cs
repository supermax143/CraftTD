using System;
using Cysharp.Threading.Tasks;
using log4net.Util;
using UnityEngine.AddressableAssets;

namespace Core.Application.Interfaces.Views
{
   public interface IViewsController
   {
      UniTask<TView> ShowView<TView>(AssetReference viewAsset) where TView : class, IView;
      void ShowView<TView>(AssetReference viewAsset, Action<TView> handler) where TView : class, IView;
   }
}
