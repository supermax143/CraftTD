using System;
using Unity.Infrastructure.Advertisement.Transactions;

namespace Unity.Infrastructure.Advertisement
{
    public interface IAdvertisementController
    {
        void AddListener<TAdvertisement>(Action<TAdvertisement> listener) where TAdvertisement : AdvertisementBase;
        void RemoveListener<TAdvertisement>(Action<TAdvertisement> listener) where TAdvertisement : AdvertisementBase;
        void ShowRewardedContinueGame();
        void ShowRewardedAddReward();
        event Action OnAdvertisementStartShow;
        void ShowFullscreen();
        void Initialize();
    }
}