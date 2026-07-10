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

        private Dictionary<EffectType, GameObject> _prefabCache = new ();
        private Dictionary<EffectType, Queue<VisualEffect>> _objectPools = new ();
        private Dictionary<VisualEffect, float> _activeEffects = new ();

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
                    _objectPools[effectAsset.effectType] = new Queue<VisualEffect>();
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
                var effect = GetFromPool(effectType, prefab, position, parent);
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

        private VisualEffect GetFromPool(EffectType effectType, GameObject prefab, Vector3 position, Transform parent)
        {
            if (_objectPools.TryGetValue(effectType, out Queue<VisualEffect> pool) && pool.Count > 0)
            {
                var effect = pool.Dequeue();
                effect.transform.position = position;
                effect.transform.SetParent(parent);
                effect.gameObject.SetActive(true);
                effect.Spawn();
                return effect;
            }

            var newEffect = Instantiate(prefab, position, Quaternion.identity, parent).GetComponent<VisualEffect>();
            return newEffect;
        }

        private void ReturnToPool(EffectType effectType, VisualEffect effect)
        {
            effect.gameObject.SetActive(false);
            
            if (!_objectPools.ContainsKey(effectType))
            {
                _objectPools[effectType] = new Queue<VisualEffect>();
            }
            
            _objectPools[effectType].Enqueue(effect);
        }

        private bool TryGetEffectTypeFromPrefab(GameObject prefab, out EffectType effectType)
        {
            var visualEffect = prefab.GetComponent<VisualEffect>();
            effectType = default;
            if (visualEffect == null)
            {
                return false;
            }
            
            effectType = visualEffect.Type;
            return true;
        }

        private void Update()
        {
            List<VisualEffect> effectsToReturn = new List<VisualEffect>();
            
            foreach (var kvp in _activeEffects)
            {
                if (Time.time >= kvp.Value)
                {
                    effectsToReturn.Add(kvp.Key);
                }
            }

            foreach (var effect in effectsToReturn)
            {
                _activeEffects.Remove(effect);
                ReturnToPool(effect.Type, effect);
            }
        }

        private void OnDestroy()
        {
            foreach (var pool in _objectPools.Values)
            {
                while (pool.Count > 0)
                {
                    var effect = pool.Dequeue();
                    if (effect != null)
                    {
                        Destroy(effect.gameObject);
                    }
                }
            }

            _prefabCache.Clear();
            _objectPools.Clear();
            _activeEffects.Clear();
        }
    }
}
