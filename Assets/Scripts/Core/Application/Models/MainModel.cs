using System.Runtime.InteropServices;
using Core.Application.Interfaces.Info;
using Zenject;

namespace Core.Application.Models
{
    using DataStorage = DataStorage.DataStorage;

    
#if DEBUG_MODE
    internal class MainModel : IMainModelInternal, IInitializable
#else
    internal class MainModel : IMainModelInternal
#endif
    {
        
        [Inject] private IChronologyInfo _chronology;
        [Inject] private DataStorage _dataStorage;
        
        
        public uint Money
        {
            get => _dataStorage.EpochData.Money;
            set => _dataStorage.EpochData.Money = value;
        }

        public EpochModel CurrentEpoch => _currentEpoch;

        private EpochModel _currentEpoch;
        
        public void Initialize()
        {
            _chronology.TryGetEpochInfo(_dataStorage.CurrentEpochIndex, out var epochInfo);
            _currentEpoch = new EpochModel(epochInfo, _dataStorage);
        }


    }
}