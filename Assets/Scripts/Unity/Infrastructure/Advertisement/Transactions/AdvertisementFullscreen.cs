using Core.Application.DataStorage;
using Zenject;

namespace Unity.Infrastructure.Advertisement.Transactions
{
    public class AdvertisementFullscreen : AdvertisementBase
    {
        [Inject] private IDataStorage _dataStorage;
        
        public override void Execute()
        {
            _api.ShowFullscreen();
        }
    }
}