using Core.Application.Models;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Application.Info.Inventory
{
    /// <summary>
    /// Конфигурация айтема инвентаря с параметрами отображения
    /// </summary>
    [CreateAssetMenu(menuName = "CraftTD/InventoryItemConfig", order = 3)]
    public class InventoryItemConfig : ScriptableObject
    {
        [SerializeField] private InventoryItemType _type;
        [SerializeField] private string _label;
        [SerializeField] private string _description;
        [SerializeField] private AssetReference _icon;

        public InventoryItemType Type => _type;
        public string Label => _label;
        public string Description => _description;
        public AssetReference Icon => _icon;

        public bool TryGetIcon(out AssetReference icon)
        {
            icon = _icon;
            return _icon != null && !string.IsNullOrEmpty(_icon.AssetGUID);
        }
    }
}
