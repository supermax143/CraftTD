namespace Core.Application.DataStorage.StorageItems
{
    /// <summary>
    /// Storage variable for unsigned integer values.
    /// Inherits from DataStorageVariable<uint> and provides uint-specific functionality.
    /// </summary>
    internal class UintStorageVariable : DataStorageVariable<uint>
    {
        public UintStorageVariable(string key, IStorageProvider storageProvider = null, uint defaultValue = 0u)
            : base(key, storageProvider, defaultValue)
        {
        }

        protected override uint LoadValue()
        {
            return _storageProvider.Get<uint>(_key, _defaultValue);
        }

        protected override void SaveValue(uint value)
        {
            _storageProvider.Set(_key, value);
        }
    }
}
