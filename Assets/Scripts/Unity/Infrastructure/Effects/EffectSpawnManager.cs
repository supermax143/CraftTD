using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Unity.Infrastructure.Effects
{
    /// <summary>
    /// Менеджер для спавна визуальных эффектов с пулом объектов и кешированием через Addressables
    /// </summary>
    public class EffectSpawnManager : MonoBehaviour
    {
        [Serializable]
        private struct EffectAsset
        {
            public EffectType effectType;
            public AssetReferenceGameObject effectPrefab;
        }

        [SerializeField]
        private EffectAsset[] _effectAssets;

        private Dictionary<EffectType, GameObject> _prefabCache = new Dictionary<EffectType, GameObject>();
        private Dictionary<EffectType, Queue<GameObject>> _objectPools = new Dictionary<EffectType, Queue<GameObject>>();
        private Dictionary<GameObject, float> _activeEffects = new Dictionary<GameObject, float>();

        [Inject]
        private void Initialize()
        {
            InitializePools();
        }

        private void InitializePools()
        {
            foreach (var effectAsset in _effectAssets)
            {
                if (!_objectPools.ContainsKey(effectAsset.effectType))
                {
                    _objectPools[effectAsset.effectType] = new Queue<GameObject>();
                }
            }
        }

        public async void SpawnEffect(EffectType effectType, float duration, Vector3 position, Transform parent = null)
        {
            if (!_prefabCache.ContainsKey(effectType))
            {
                await LoadPrefabAsync(effectType);
            }

            if (_prefabCache.TryGetValue(effectType, out GameObject prefab))
            {
                GameObject effect = GetFromPool(effectType, prefab, position, parent);
                _activeEffects[effect] = Time.time + duration;
            }
        }

        private async Task LoadPrefabAsync(EffectType effectType)
        {
            foreach (var effectAsset in _effectAssets)
            {
                if (effectAsset.effectType == effectType)
                {
                    GameObject prefab = await effectAsset.effectPrefab.LoadAssetAsync<GameObject>().Task;
                    _prefabCache[effectType] = prefab;
                    break;
                }
            }
        }

        private GameObject GetFromPool(EffectType effectType, GameObject prefab, Vector3 position, Transform parent)
        {
            if (_objectPools.TryGetValue(effectType, out Queue<GameObject> pool) && pool.Count > 0)
            {
                GameObject effect = pool.Dequeue();
                effect.transform.position = position;
                effect.transform.SetParent(parent);
                effect.SetActive(true);
                return effect;
            }

            GameObject newEffect = Instantiate(prefab, position, Quaternion.identity, parent);
            return newEffect;
        }

        private void ReturnToPool(EffectType effectType, GameObject effect)
        {
            effect.SetActive(false);
            
            if (!_objectPools.ContainsKey(effectType))
            {
                _objectPools[effectType] = new Queue<GameObject>();
            }
            
            _objectPools[effectType].Enqueue(effect);
        }

        private bool TryGetEffectTypeFromPrefab(GameObject prefab, out EffectType effectType)
        {
            effectType = default;
            foreach (var kvp in _prefabCache)
            {
                if (kvp.Value == prefab)
                {
                    effectType = kvp.Key;
                    return true;
                }
            }
            return false;
        }

        private void Update()
        {
            List<GameObject> effectsToReturn = new List<GameObject>();
            
            foreach (var kvp in _activeEffects)
            {
                if (Time.time >= kvp.Value)
                {
                    effectsToReturn.Add(kvp.Key);
                }
            }

            foreach (var effect in effectsToReturn)
            {
                if (!TryGetEffectTypeFromPrefab(effect, out var effectType))
                {
                    Debug.LogError($"{this.GetType().Name} effect {effect} not found");
                    continue;
                }
                _activeEffects.Remove(effect);
                ReturnToPool(effectType, effect);
            }
        }

        private void OnDestroy()
        {
            foreach (var pool in _objectPools.Values)
            {
                while (pool.Count > 0)
                {
                    GameObject effect = pool.Dequeue();
                    if (effect != null)
                    {
                        Destroy(effect);
                    }
                }
            }

            _prefabCache.Clear();
            _objectPools.Clear();
            _activeEffects.Clear();
        }
    }
}
