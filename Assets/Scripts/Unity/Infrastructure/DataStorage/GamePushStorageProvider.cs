using System.Linq;
using System.Threading.Tasks;
using Core.Application.DataStorage;
using GamePush;
using UnityEngine;

namespace Zombies
{
    /// <summary>
    /// Implementation of IStorageProvider using GamePush GP_Player.
    /// Provides persistent storage using GamePush cloud player data system.
    /// </summary>
    public class GamePushStorageProvider : IGlobalStorageProvider
    {
        private TaskCompletionSource<bool> _initTask;

        
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
                return (T)(object)GP_Player.GetInt(key);
            else if (type == typeof(float))
                return (T)(object)GP_Player.GetFloat(key);
            else if (type == typeof(string))
                return (T)(object)GP_Player.GetString(key);
            else if (type == typeof(bool))
                return (T)(object)GP_Player.GetBool(key);
            else if (type == typeof(uint))
                return (T)(object)(uint)GP_Player.GetInt(key);
            else
                throw new System.NotSupportedException($"Type {type} is not supported by GamePushStorageProvider");
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
                GP_Player.Set(key, (int)(object)value);
            else if (type == typeof(float))
                GP_Player.Set(key, (float)(object)value);
            else if (type == typeof(string))
                GP_Player.Set(key, (string)(object)value);
            else if (type == typeof(bool))
                GP_Player.Set(key, (bool)(object)value);
            else if (type == typeof(uint))
                GP_Player.Set(key, (int)(uint)(object)value);
            else
                throw new System.NotSupportedException($"Type {type} is not supported by GamePushStorageProvider");
            
            Debug.Log($"{this.GetType().Name} Set value: {key}: {value}");
            GP_Player.Sync();
        }

        /// <summary>
        /// Checks if a value exists for the specified key.
        /// </summary>
        /// <param name="key">The unique key identifier</param>
        /// <returns>True if key exists, false otherwise</returns>
        public bool HasKey(string key)
        {
            return GP_Player.Has(key);
        }

        public void Reset()
        {
            GP_Player.ResetPlayer();
            GP_Player.FetchFields();
        }

        
    }
}
