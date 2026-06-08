using System.Runtime.InteropServices;
using Zenject;

namespace Core.Application.Models
{
    using DataStorage = DataStorage.DataStorage;

    internal class MainModel : IMainModelInternal
    {
        
        [Inject] private DataStorage _dataStorage;
        
        
        public uint Money
        {
            get => _dataStorage.EpochData.Money;
            set => _dataStorage.EpochData.Money = value;
        }
        
        public void Initialize()
        {
            
            
        }
        
        
    }
}