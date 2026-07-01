using System.Collections.Generic;
using UnityEngine;

namespace Core.Application.Info.Shop
{
    /// <summary>
    /// Контейнер конфигураций айтемов магазина
    /// </summary>
    [CreateAssetMenu(menuName = "CraftTD/ShopConfig", order = 1)]
    public class ShopConfig : ScriptableObject
    {
        [SerializeField] private List<ShopItemConfig> _items;

        public IReadOnlyList<ShopItemConfig> Items => _items;
    }
}
