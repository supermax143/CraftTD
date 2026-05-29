using System.Threading.Tasks;
using Core.Application.DataStorage;
using UnityEngine;

namespace Unity.Infrastructure.DataStorage
{
    /// <summary>
    /// Implementation of IStorageProvider using Unity's PlayerPrefs.
    /// Provides persistent storage using Unity's built-in preference system.
    /// </summary>
    public class PlayerPrefsStorageProvider : ILocalStorageProvider, IGlobalStorageProvider
    {
        /// <summary>
        /// Retrieves a value of type T associated with the specified key.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve</typeparam>
        /// <param name="key">The unique key identifier</param>
        /// <returns>The stored value or default(T) if not found</returns>
        public T Get<T>(string key)
        {
            return Get<T>(key, default(T));
        }

        /// <summary>
        /// Retrieves a value of type T associated with the specified key, with custom default value.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve</typeparam>
        /// <param name="key">The unique key identifier</param>
        /// <param name="defaultValue">The default value to return if key doesn't exist</param>
        /// <returns>The stored value or defaultValue if not found</returns>
        public T Get<T>(string key, T defaultValue)
        {
            if (!HasKey(key))
                return defaultValue;

            var type = typeof(T);
            
            if (type == typeof(int))
                return (T)(object)PlayerPrefs.GetInt(key);
            else if (type == typeof(float))
                return (T)(object)PlayerPrefs.GetFloat(key);
            else if (type == typeof(string))
                return (T)(object)PlayerPrefs.GetString(key);
            else if (type == typeof(bool))
                return (T)(object)(PlayerPrefs.GetInt(key) != 0);
            else if (type == typeof(uint))
                return (T)(object)(uint)PlayerPrefs.GetInt(key);
            else
                throw new System.NotSupportedException($"Type {type} is not supported by PlayerPrefsStorageProvider");
        }

        /// <summary>
        /// Stores a value of type T associated with the specified key.
        /// </summary>
        /// <typeparam name="T">The type of value to store</typeparam>
        /// <param name="key">The unique key identifier</param>
        /// <param name="value">The value to store</param>
        public void Set<T>(string key, T value)
        {
            var type = typeof(T);
            
            if (type == typeof(int))
                PlayerPrefs.SetInt(key, (int)(object)value);
            else if (type == typeof(float))
                PlayerPrefs.SetFloat(key, (float)(object)value);
            else if (type == typeof(string))
                PlayerPrefs.SetString(key, (string)(object)value);
            else if (type == typeof(bool))
                PlayerPrefs.SetInt(key, (bool)(object)value ? 1 : 0);
            else if (type == typeof(uint))
                PlayerPrefs.SetInt(key, (int)(uint)(object)value);
            else
                throw new System.NotSupportedException($"Type {type} is not supported by PlayerPrefsStorageProvider");
        }

        /// <summary>
        /// Checks if a value exists for the specified key.
        /// </summary>
        /// <param name="key">The unique key identifier</param>
        /// <returns>True if key exists, false otherwise</returns>
        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void Reset(){}
        public Task Initialize()
        {
            return Task.CompletedTask;
        }
    }
}
