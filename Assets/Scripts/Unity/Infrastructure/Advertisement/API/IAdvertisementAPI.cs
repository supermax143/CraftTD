using System;

namespace Unity.Infrastructure.Advertisement.API
{
    public interface IAdvertisementAPI
    {
        event Action OnRewardedVideoFinished;
        event Action<string> OnRewardedVideo;
        event Action OnRewardedVideoError;
        void ShowRewardedAdv(string id);
        void ShowFullscreen();
        event Action OnFullscreenClose;
        void RemoveAllAds();
    }
}