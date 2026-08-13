using Core.Application.Info.Inventory;
using Core.Application.Info.Shop;
using Core.Application.Interfaces;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.HUD
{
    /// <summary>
    /// Контейнер для отображения награды (ресурс или айтем инвентаря)
    /// </summary>
    public class RewardContainer : MonoBehaviour
    {
        [SerializeField]
        private ResourceContainer _resourceContainer;
        [SerializeField]
        private InventoryItemContainer _inventoryItemContainer;

        [Inject] private IInventoryModel _inventory;

        public async void SetReward(Reward reward)
        {
            switch (reward.RewardType)
            {
                case RewardType.Resource:
                    _resourceContainer.gameObject.SetActive(true);
                    _inventoryItemContainer.gameObject.SetActive(false);
                    _resourceContainer.SetResource(new Resource(reward.ResourceType, reward.Count));
                    break;

                case RewardType.Item:
                    _resourceContainer.gameObject.SetActive(false);
                    _inventoryItemContainer.gameObject.SetActive(true);
                    if (_inventory.TryGetItemConfig(reward.ItemType, out var itemConfig))
                    {
                        await _inventoryItemContainer.SetItemConfig(itemConfig);
                    }
                    break;
            }
        }
    }
}
