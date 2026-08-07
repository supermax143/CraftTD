using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Unity.Presentation.Components.TabBar.ViewsTabBar
{
    public class ViewsTabBar : TabBar<AssetReference>
    {
        public override void SelectTab(TabBarButton<AssetReference> tab)
        {
            base.SelectTab(tab);
            Debug.Log(tab.Value.AssetGUID);
        }
    }
}