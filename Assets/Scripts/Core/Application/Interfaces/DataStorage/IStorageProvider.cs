namespace Core.Application.DataStorage
{
    /// <summary>
    /// Interface for abstracting data storage mechanisms.
    /// Provides generic methods for storing and retrieving typed data.
    /// </summary>
    public interface IStorageProvider
    {
        T Get<T>(string key);

        T Get<T>(string key, T defaultValue);

        void Set<T>(string key, T value);

        bool HasKey(string key);

        void Reset();
    }

    public interface ILocalStorageProvider : IStorageProvider
    {
    }
    
    public interface IGlobalStorageProvider : IStorageProvider
    {
    }

}
