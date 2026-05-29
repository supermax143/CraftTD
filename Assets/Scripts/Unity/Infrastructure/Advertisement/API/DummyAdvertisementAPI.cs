using System;

namespace Unity.Infrastructure.Advertisement.API
{
    public class DummyAdvertisementAPI : IAdvertisementAPI
    {
        public event Action OnRewardedVideoFinished;
        public event Action<string> OnRewardedVideo;
        public event Action OnRewardedVideoError;
        public event Action OnFullscreenClose;
        public void RemoveAllAds()
        {
            
        }

        public void ShowRewardedAdv(string id)
        {
            OnRewardedVideoFinished?.Invoke();
            OnRewardedVideo?.Invoke(id);
        }

        public void ShowFullscreen()
        {
            OnFullscreenClose?.Invoke();
        }

    }
}