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

        public async Task Init()
        {
            _chronology.TryGetEpochInfo(_dataStorage.CurrentEpochIndex, out var epochInfo);
            _epoch = new EpochModel(epochInfo, _dataStorage.EpochData, _gameStats);
        }
        
    }
}