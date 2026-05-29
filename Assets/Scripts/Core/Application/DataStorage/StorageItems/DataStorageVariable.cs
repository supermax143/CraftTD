using System;

namespace Core.Application.DataStorage.StorageItems
{
    /// <summary>
    /// Abstract base class for typed storage variables.
    /// Provides type-safe storage operations with change notifications.
    /// </summary>
    /// <typeparam name="T">The type of value to store</typeparam>
    internal abstract class DataStorageVariable<T>
    {
        protected readonly IStorageProvider _storageProvider;
        protected readonly string _key;
        protected readonly T _defaultValue;
        protected T _cachedValue;
        protected bool _isLoaded;
        
        public event Action<T> ValueChanged;

        protected DataStorageVariable(string key, IStorageProvider storageProvider, T defaultValue = default)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _defaultValue = defaultValue;
            if (storageProvider == null)
                throw new ArgumentNullException(nameof(storageProvider), "Storage provider cannot be null.");
            _storageProvider = storageProvider;
        }

        public T GetValue()
        {
            if (!_isLoaded)
            {
                _cachedValue = LoadValue();
                _isLoaded = true;
            }
            return _cachedValue;
        }

        public T Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        public void SetValue(T value)
        {
            if (Equals(_cachedValue, value) && _isLoaded)
                return;

            _cachedValue = value;
            _isLoaded = true;
            SaveValue(value);
            ValueChanged?.Invoke(value);
        }

        protected abstract T LoadValue();

        protected abstract void SaveValue(T value);

        public string Key => _key;
    }
}
