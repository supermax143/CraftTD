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

        public EpochModel PlayerEpoch => _playerEpoch;
        public EpochModel EnemyEpoch => _enemyEpoch;
        
        private EpochModel _playerEpoch;
        private EpochModel _enemyEpoch;

        public Resource Money
        {
            get => _playerEpoch.Money;
            set => _playerEpoch.Money = value;
        }

#if DEBUG_MODE
        public void Initialize()
        {
            Init();
        }
#endif

        public void CompleteEpoch()
        {
            var cost = Resource.Money(GetEpochCompleteCost());
            if (cost > Money)
            {
                return;
            }
            Money -= cost;
            _dataStorage.SetEpochIndex(_dataStorage.CurrentEpochIndex + 1);
            _dataStorage.EpochData.Reset();
            Init();
        }

        public int CurrentEpochNumber => _dataStorage.CurrentEpochIndex + 1;


        public int GetEpochCompleteCost()
        {
            return _gameStats.GetEpochCompleteCost(CurrentEpochNumber);
        }
        
        public bool HasNextEpoch() => 
            _dataStorage.CurrentEpochIndex < _chronology.Epochs.Count-1;
        
        
        public void Init()
        {
            _chronology.TryGetEpochInfo(_dataStorage.CurrentEpochIndex, out var epochInfo);
            _playerEpoch = new EpochModel( Faction.Player, CurrentEpochNumber ,epochInfo, _dataStorage.EpochData, _gameStats);
            _enemyEpoch = new EpochModel( Faction.Enemy, CurrentEpochNumber ,epochInfo, _dataStorage.EpochData, _gameStats);
        }
        
        public void Reset()
        {
            _dataStorage.Reset();
            Init();
        }
        
    }
}