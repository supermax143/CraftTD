namespace Unity.Infrastructure.Advertisement.Transactions
{
    public class AdvertisementContinueGame : AdvertisementBase
    {
        public override bool Rewarded => true;

        public override void Execute()
        {
            _api.ShowRewardedAdv("ContinueGame");
        }
    }
}