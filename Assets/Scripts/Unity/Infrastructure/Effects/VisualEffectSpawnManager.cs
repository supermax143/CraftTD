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
    public class VisualEffectSpawnManager : MonoBehaviour
    {
        [Serializable]
        private struct EffectAsset
        {
            public VisualEffectType visualEffectType;
            public AssetReferenceGameObject effectPrefab;
        }

        [SerializeField]
        private EffectAsset[] _effectAssets;
        [SerializeField]
        private TextBubbleHelper _textBubbleHelper;
        
        private Dictionary<VisualEffectType, GameObject> _prefabCache = new ();
        private Dictionary<VisualEffectType, Queue<VisualEffect>> _objectPools = new ();

        [Inject]
        private void Initialize()
        {
            InitializePools();
        }

        private void InitializePools()
        {
            foreach (var effectAsset in _effectAssets)
            {
                if (!_objectPools.ContainsKey(effectAsset.visualEffectType))
                {
                    _objectPools[effectAsset.visualEffectType] = new Queue<VisualEffect>();
                }
            }
        }

        public async Task<VisualEffect> SpawnRandomHitBubble(Vector3 position,
            Transform parent = null)
        {
            var visualEffectType = _textBubbleHelper.GetRandomBubbleType();
            var bubble = await SpawnEffect(visualEffectType, position, parent, false) as TextBubbleVisualEffect;
            bubble.SetText(_textBubbleHelper.GetRandomBubbleText());
            bubble.Spawn();
            return bubble;
        }

        public async Task<VisualEffect> SpawnEffect(VisualEffectType visualEffectType, Vector3 position, 
            Transform parent = null, bool spawn = true)
        {
            if (!_prefabCache.ContainsKey(visualEffectType))
            {
                await LoadPrefabAsync(visualEffectType);
            }

            if (!_prefabCache.TryGetValue(visualEffectType, out GameObject prefab))
            {
                Debug.Log($"{GetType().Name} effect not found: {visualEffectType}");
                return null;
            }
            var effect = GetFromPool(visualEffectType, prefab, position, parent);
            effect.OnComplete += ReturnToPool;
            if (spawn)
            {
                effect.Spawn();
            }
            return effect;
        }

        private async Task LoadPrefabAsync(VisualEffectType visualEffectType)
        {
            foreach (var effectAsset in _effectAssets)
            {
                if (effectAsset.visualEffectType == visualEffectType)
                {
                    GameObject prefab = await effectAsset.effectPrefab.LoadAssetAsync<GameObject>().Task;
                    _prefabCache[visualEffectType] = prefab;
                    break;
                }
            }
        }

        private VisualEffect GetFromPool(VisualEffectType visualEffectType, GameObject prefab, Vector3 position, Transform parent)
        {
            if (_objectPools.TryGetValue(visualEffectType, out Queue<VisualEffect> pool) && pool.Count > 0)
            {
                var effect = pool.Dequeue();
                effect.transform.position = position;
                effect.transform.SetParent(parent);
                return effect;
            }

            var newEffect = Instantiate(prefab, position, Quaternion.identity, parent).GetComponent<VisualEffect>();
            newEffect.gameObject.SetActive(false);
            return newEffect;
        }

        private void ReturnToPool(VisualEffect effect)
        {
            effect.OnComplete -= ReturnToPool;
            effect.gameObject.SetActive(false);
            
            if (!_objectPools.ContainsKey(effect.Type))
            {
                _objectPools[effect.Type] = new Queue<VisualEffect>();
            }
            
            _objectPools[effect.Type].Enqueue(effect);
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
        }
    }
}
