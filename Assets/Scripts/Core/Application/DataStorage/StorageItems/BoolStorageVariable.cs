namespace Core.Application.DataStorage.StorageItems
{
    /// <summary>
    /// Storage variable for boolean values.
    /// Inherits from DataStorageVariable<bool> and provides bool-specific functionality.
    /// </summary>
    internal class BoolStorageVariable : DataStorageVariable<bool>
    {
        public BoolStorageVariable(string key, IStorageProvider storageProvider, bool defaultValue = false)
            : base(key, storageProvider, defaultValue)
        {
        }

        protected override bool LoadValue()
        {
            return _storageProvider.Get<bool>(_key, _defaultValue);
        }

        protected override void SaveValue(bool value)
        {
            _storageProvider.Set(_key, value);
        }
    }
}
