using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.Game;
using Zenject;

namespace Core.Application.Models
{
    using DataStorage = DataStorage.DataStorage;

    
#if DEBUG_MODE
    internal class MainModel : IMainModelInternal, IInitializable
#else
    internal class MainModel : IMainModelInternal, IBootstrapStep
#endif
    {
        
        [Inject] private ChronologyInfo _chronology;
        [Inject] private DataStorage _dataStorage;
        [Inject] private GameStats _gameStats;

        public EpochModel Epoch => _epoch;
        private EpochModel _epoch;

        public uint Money
        {
            get => _epoch.Money;
            set => _epoch.Money = value;
        }

#if DEBUG_MODE
        public void Initialize()
        {
            Init();
        }
#endif

        public void CompleteEpoch()
        {
            if (GetEpochCompleteCost() < Money)
            {
                return;
            }
            _dataStorage.SetEpochIndex(_dataStorage.CurrentEpochIndex + 1);
        }

        public int GetEpochCompleteCost()
        {
            return _gameStats.GetEpochCompleteCost(_dataStorage.CurrentEpochIndex + 1);
        }
        
        public bool HasNextEpoch() => 
            _dataStorage.CurrentEpochIndex < _chronology.Epochs.Count-1;
        
        
        public void Init()
        {
            _chronology.TryGetEpochInfo(_dataStorage.CurrentEpochIndex, out var epochInfo);
            _epoch = new EpochModel(epochInfo, _dataStorage.EpochData, _gameStats);
        }
        
        public void Reset()
        {
            _dataStorage.Reset();
            Init();
        }
        
    }
}