using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Unity.Infrastructure.ResourceManager
{
    public class HandleStorage : IDisposable
    {
        private readonly Dictionary<AsyncOperationHandle, HashSet<string>> _taggedHandles = new(100);
        private readonly Dictionary<AsyncOperationHandle, HashSet<GameObject>> _linkedHandles = new(100);

        private readonly List<AsyncOperationHandle> _handlesToRemove = new(100);
        
        public Task<T> RegisterHandle<T>(ref AsyncOperationHandle<T> handle, HandleDependencies dependencies)
        {
            if (!dependencies.IsValid())
                throw new ArgumentException($"Can't link handle to empty dependencies");

            RegisterHandleLinks(handle, dependencies.Links);
            RegisterHandleTags(handle, dependencies.Tags);

            return handle.Task;
        }

        public Task<T> RegisterHandle<T>(ref AsyncOperationHandle<T> handle, GameObject link)
        {
            if (!_linkedHandles.TryGetValue(handle, out var links))
            {
                links = _linkedHandles[handle] = new HashSet<GameObject>();
            }

            links.Add(link);

            var h = handle;
            if (!link.TryGetComponent<ResourceStorageLifetimeHandler>(out var c))
                c = link.AddComponent<ResourceStorageLifetimeHandler>();
            c.AppendCallback(ReleaseLinkCallback);
            void ReleaseLinkCallback() => ReleaseLink(h, link, links);

            return handle.Task;
        }

        public Task<T> RegisterHandle<T>(ref AsyncOperationHandle<T> handle, string tag)
        {
            if (string.IsNullOrEmpty(tag))
                throw new ArgumentException(nameof(tag));

            if (!_taggedHandles.TryGetValue(handle, out var tags))
            {
                tags = _taggedHandles[handle] = new HashSet<string>();
            }

            tags.Add(tag);

            return handle.Task;
        }

        public void ReleaseTag(string tag)
        {
            if (tag == null)
                return;

            _handlesToRemove.Clear();

            foreach (var kvp in _taggedHandles)
            {
                if (kvp.Value.Remove(tag) && kvp.Value.Count <= 0)
                {
                    _handlesToRemove.Add(kvp.Key);
                }
            }

            foreach (var handle in _handlesToRemove)
            {
                _taggedHandles.Remove(handle);

                if (!_linkedHandles.ContainsKey(handle))
                    ReleaseHandle(handle);
            }
        }

        private void RegisterHandleTags(AsyncOperationHandle handle, IReadOnlyList<string> newTags)
        {
            if (newTags is null || newTags.Count <= 0)
                return;

            if (!_taggedHandles.TryGetValue(handle, out var tags))
            {
                tags = _taggedHandles[handle] = new HashSet<string>();
            }

            foreach (var tag in newTags)
            {
                tags.Add(tag);
            }
        }
        
        private void RegisterHandleLinks(AsyncOperationHandle handle, IReadOnlyList<GameObject> newLinks)
        {
            if (newLinks is null || newLinks.Count <= 0)
                return;

            if (!_linkedHandles.TryGetValue(handle, out var links))
            {
                links = _linkedHandles[handle] = new HashSet<GameObject>();
            }

            foreach (var link in newLinks)
            {
                links.Add(link);
                if (!link.TryGetComponent<ResourceStorageLifetimeHandler>(out var c))
                    c = link.AddComponent<ResourceStorageLifetimeHandler>();
                c.AppendCallback(ReleaseLinkCallback);
                void ReleaseLinkCallback() => ReleaseLink(handle, link, links);
            }
        }
        
        private void ReleaseLink(AsyncOperationHandle handle, GameObject link, HashSet<GameObject> links)
        {
            _handlesToRemove.Clear();

            if (links.Remove(link) && links.Count <= 0)
            {
                _linkedHandles.Remove(handle);

                if (!_taggedHandles.ContainsKey(handle))
                    ReleaseHandle(handle);
            }
        }

        public void Dispose()
        {
            foreach (var key in _linkedHandles.Keys)
            {
                ReleaseHandle(key);
            }

            foreach (var key in _taggedHandles.Keys)
            {
                ReleaseHandle(key);
            }

            _linkedHandles.Clear();
            _taggedHandles.Clear();
        }

        private void ReleaseHandle(AsyncOperationHandle handle)
        {
            if (!handle.IsValid())
            {
                return;
            }
        
            try
            {
                Addressables.Release(handle);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to release handle: {ex.Message}");
            }
        }
    }
}
