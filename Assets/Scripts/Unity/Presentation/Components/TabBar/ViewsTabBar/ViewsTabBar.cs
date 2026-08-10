using Core.Application.Interfaces.Views;
using Unity.Presentation.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Unity.Presentation.Components.TabBar.ViewsTabBar
{
    public class ViewsTabBar : TabBar<AssetReference>
    {
        [Inject] private IViewsController _viewsController;
        
        
        public override void SelectTab(TabBarButton<AssetReference> tab)
        {
            base.SelectTab(tab);
            _viewsController.ShowView<ViewBase>(tab.Value, view =>
            {
                if (view == null)
                {
                    return;
                }
                view.Show();
            });
        }
    }
}