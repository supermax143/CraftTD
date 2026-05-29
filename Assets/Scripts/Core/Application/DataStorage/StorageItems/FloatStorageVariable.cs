namespace Core.Application.DataStorage.StorageItems
{
    /// <summary>
    /// Storage variable for float values.
    /// Inherits from DataStorageVariable<float> and provides float-specific functionality.
    /// </summary>
    internal class FloatStorageVariable : DataStorageVariable<float>
    {
        public FloatStorageVariable(string key, IStorageProvider storageProvider, float defaultValue = 0.0f)
            : base(key, storageProvider, defaultValue)
        {
        }

        protected override float LoadValue()
        {
            return _storageProvider.Get<float>(_key, _defaultValue);
        }

        protected override void SaveValue(float value)
        {
            _storageProvider.Set(_key, value);
        }
    }
}
