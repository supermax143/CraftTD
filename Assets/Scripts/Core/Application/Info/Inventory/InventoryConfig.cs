using System.Collections.Generic;
using UnityEngine;

namespace Core.Application.Info.Inventory
{
    /// <summary>
    /// Контейнер конфигураций айтемов инвентаря
    /// </summary>
    [CreateAssetMenu(menuName = "CraftTD/InventoryConfig", order = 2)]
    public class InventoryConfig : ScriptableObject
    {
        [SerializeField] private List<InventoryItemConfig> _items;

        public IReadOnlyList<InventoryItemConfig> Items => _items;
    }
}
