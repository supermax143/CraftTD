namespace Unity.Infrastructure.Advertisement.Transactions
{
    public class AdvertisementDoubleReward : AdvertisementBase
    {
        
        public override bool Rewarded => true;
        
        public override void Execute()
        {
            _api.ShowRewardedAdv("double money");
        }
    }
}