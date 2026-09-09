namespace Core.Application.DataStorage.StorageItems
{
    /// <summary>
    /// Storage variable for string values.
    /// Inherits from DataStorageVariable<string> and provides string-specific functionality.
    /// </summary>
    internal class StringStorageVariable : DataStorageVariable<string>
    {
        public StringStorageVariable(string key, IStorageProvider storageProvider, string defaultValue = null)
            : base(key, storageProvider, defaultValue)
        {
        }

        protected override string LoadValue()
        {
            return _storageProvider.Get<string>(_key, _defaultValue);
        }

        protected override void SaveValue(string value)
        {
            _storageProvider.Set(_key, value);
        }
    }
}
