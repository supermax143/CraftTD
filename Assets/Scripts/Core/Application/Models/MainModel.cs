using System;
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
        
        public event Action OnEnemyEpochChanged;
        public event Action OnPlayerEpochChanged;
        
        [Inject] private ChronologyInfo _chronology;
        [Inject] private DataStorage _dataStorage;
        [Inject] private GameStats _gameStats;

        public EpochModel PlayerEpoch => _playerEpoch;
        public EpochModel EnemyEpoch => _enemyEpoch;
        
        private EpochModel _playerEpoch;
        private EpochModel _enemyEpoch;

        private int _selectedEnemyEpochIndex;
        
        public Resource Money
        {
            get => _playerEpoch.Money;
            set => _playerEpoch.Money = value;
        }
        public int CurrentPlayerEpochNumber => _dataStorage.CurrentPlayerEpochIndex + 1;
        public int CurrentEnemyEpochNumber => _dataStorage.CurrentEnemyEpochIndex + 1;
        public int SelectedEnemyEpochIndex => _selectedEnemyEpochIndex;

#if DEBUG_MODE
        public void Initialize()
        {
            Init();
        }
#endif

        public void CompleteEpoch()
        {
            _dataStorage.SetPlayerEpochIndex(_dataStorage.CurrentPlayerEpochIndex + 1);
            _dataStorage.EpochData.Reset();
            _dataStorage.SetEnemyEpochIndex(0);
            _playerEpoch = GetEpochModel(_dataStorage.CurrentPlayerEpochIndex, Faction.Player);
            SelectEnemyEpochIndex(_dataStorage.CurrentEnemyEpochIndex);
            OnPlayerEpochChanged?.Invoke();
        }
        
        public void CompleteEpochForMoney()
        {
            var cost = Resource.Money(GetEpochCompleteCost());
            if (cost > Money)
            {
                return;
            }
            Money -= cost;
            CompleteEpoch();
        }

        public void IncreaseEnemyEpoch()
        {
            _dataStorage.SetEnemyEpochIndex(_dataStorage.CurrentEnemyEpochIndex + 1);
            SelectEnemyEpochIndex(_dataStorage.CurrentEnemyEpochIndex);
        }

        public int GetEpochCompleteCost()
        {
            return _gameStats.GetEpochCompleteCost(CurrentPlayerEpochNumber);
        }
        
        public bool HasNextEpoch() => 
            _dataStorage.CurrentPlayerEpochIndex < _chronology.Epochs.Count-1;
        
        public void SelectEnemyEpochIndex(int index)
        {
            if (index < 0 || 
                index > CurrentPlayerEpochNumber - 1 ||
                index > CurrentEnemyEpochNumber - 1)
            {
                return;
            }
            _selectedEnemyEpochIndex = index;
            _enemyEpoch = GetEpochModel(_selectedEnemyEpochIndex, Faction.Enemy);
            OnEnemyEpochChanged?.Invoke();
        }
        
        public void Init()
        {
            _playerEpoch = GetEpochModel(_dataStorage.CurrentPlayerEpochIndex, Faction.Player);
            var selectedEnemyEpochIndex = 
                _dataStorage.CurrentEnemyEpochIndex > _dataStorage.CurrentPlayerEpochIndex ? 
                    _dataStorage.CurrentPlayerEpochIndex : 
                    _dataStorage.CurrentEnemyEpochIndex;

            SelectEnemyEpochIndex(selectedEnemyEpochIndex);
        }

        private EpochModel GetEpochModel(int index, Faction faction)
        {
            _chronology.TryGetEpochInfo(index, out var epochInfo);
            return new EpochModel(faction, index + 1, epochInfo, _dataStorage.EpochData, _gameStats);
        }
        
        public void Reset()
        {
            _dataStorage.Reset();
            Init();
        }
        
    }
}