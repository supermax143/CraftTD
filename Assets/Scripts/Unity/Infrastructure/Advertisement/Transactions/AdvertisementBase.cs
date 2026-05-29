using System;
using Unity.Infrastructure.Advertisement.API;
using Zenject;

namespace Unity.Infrastructure.Advertisement.Transactions
{
    public abstract class AdvertisementBase : IInitializable
    {
        public event Action<AdvertisementBase> Completed;
        
        [Inject] protected IAdvertisementAPI _api;

        public virtual bool Rewarded => false;
        
        public enum Result
        {
            Completed,
            Failed
        }
        
        public Result AdvResult { get; private set; }

        public void Initialize()
        {
            _api.OnRewardedVideo += OnRewardedVideo;
            _api.OnRewardedVideoError += OnRewardedVideoError;
            _api.OnRewardedVideoFinished += OnRewardedVideoFinished;
            _api.OnFullscreenClose += OnFullscreenClose;
        }

        private void OnFullscreenClose() => DispatchResult(Result.Completed);

        private void OnRewardedVideoFinished() => DispatchResult(Result.Completed);

        private void OnRewardedVideoError() => DispatchResult(Result.Failed);

        private void OnRewardedVideo(string id) => DispatchResult(Result.Completed);

        protected void DispatchResult(Result result)
        {
            AdvResult = result;
            Completed?.Invoke(this);
        }
        
        public abstract void Execute();
       
    }
}