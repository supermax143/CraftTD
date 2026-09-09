namespace Core.Application.DataStorage.StorageItems
{
    /// <summary>
    /// Storage variable for integer values.
    /// Inherits from DataStorageVariable<int> and provides int-specific functionality.
    /// </summary>
    internal class IntStorageVariable : DataStorageVariable<int>
    {
        public IntStorageVariable(string key, IStorageProvider storageProvider, int defaultValue = 0)
            : base(key, storageProvider, defaultValue)
        {
        }

        protected override int LoadValue()
        {
            return _storageProvider.Get<int>(_key, _defaultValue);
        }

        protected override void SaveValue(int value)
        {
            _storageProvider.Set(_key, value);
        }
    }
}
