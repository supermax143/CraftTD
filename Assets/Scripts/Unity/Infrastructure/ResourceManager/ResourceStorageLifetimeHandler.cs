using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Infrastructure.ResourceManager
{
    internal class ResourceStorageLifetimeHandler : MonoBehaviour
    {
        private readonly List<Action> _callbacks = new();

        public void AppendCallback(Action callback) => _callbacks.Add(callback);

        private void OnDestroy()
        {
            foreach (var callback in _callbacks) 
                callback?.Invoke();
        }
    }
}