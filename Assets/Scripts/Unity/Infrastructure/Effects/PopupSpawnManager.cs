using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Utils.Time;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Random = UnityEngine.Random;

namespace Unity.Infrastructure.Effects
{
    /// <summary>
    /// Менеджер для спавна визуальных эффектов с пулом объектов и кешированием через Addressables
    /// </summary>
    public class PopupSpawnManager : MonoBehaviour
    {
        [Serializable]
        private struct EffectAsset
        {
            public PopupType popupType;
            public AssetReferenceGameObject effectPrefab;
        }

        [SerializeField]
        private EffectAsset[] _effectAssets;
        [SerializeField]
        private TextParticleHelper textParticleHelper;
        [SerializeField]
        private float _textBubbleCooldown = .5f;
        [SerializeField]
        private ExplosionHelper _explosionHelper;
        [SerializeField]
        private float _explosionCooldown = .5f;
        [SerializeField]
        private Transform _effectsContainer;
        [SerializeField]
        private Transform _popupsContainer;
        
        private Dictionary<PopupType, GameObject> _prefabCache = new ();
        private Dictionary<PopupType, Queue<Popup>> _objectPools = new ();

        private Timer _textBubbleCooldownTimer = new();
        private Timer _explosionCooldownTimer = new();
        
        private void Initialize()
        {
            InitializePools();
        }

        private void InitializePools()
        {
            foreach (var effectAsset in _effectAssets)
            {
                if (!_objectPools.ContainsKey(effectAsset.popupType))
                {
                    _objectPools[effectAsset.popupType] = new Queue<Popup>();
                }
            }
        }

        public async Task<Popup> SpawnRandomHitBubble(Vector3 position,
            Transform parent = null, Vector2 deltaX = default,Vector2 deltaY = default)
        {
            if (!_textBubbleCooldownTimer.IsComplete)
            {
                return null;
            }
            var randomDelta = new Vector2(Random.Range(deltaX.x, deltaX.y), Random.Range(deltaY.x, deltaY.y));
            position += new Vector3(randomDelta.x, randomDelta.y, -10);
            var visualEffectType = textParticleHelper.GetRandomBubbleType();
            var bubble = await SpawnEffect(visualEffectType, position, parent, false) as TextParticlePopup;
            bubble.SetText(textParticleHelper.GetRandomBubbleText());
            bubble.Spawn();
            _textBubbleCooldownTimer.Start(_textBubbleCooldown);
            return bubble;
        }

        public async Task<Popup> SpawnRandomExplosion(Vector3 position,
            Transform parent = null, Vector2 deltaX = default, Vector2 deltaY = default)
        {
            if (!_explosionCooldownTimer.IsComplete)
            {
                return null;
            }
            var randomDelta = new Vector2(Random.Range(deltaX.x, deltaX.y), Random.Range(deltaY.x, deltaY.y));
            position += new Vector3(randomDelta.x, randomDelta.y, -10);
            var visualEffectType = _explosionHelper.GetRandomExplosionType();
            var explosion = await SpawnEffect(visualEffectType, position, parent, true);
            _explosionCooldownTimer.Start(_explosionCooldown);
            return explosion;
        }

        public async Task<Popup> SpawnEffect(PopupType popupType, Vector3 position, 
            Transform parent = null, bool spawn = true)
        {
            if (!_prefabCache.ContainsKey(popupType))
            {
                await LoadPrefabAsync(popupType);
            }

            if (!_prefabCache.TryGetValue(popupType, out GameObject prefab))
            {
                Debug.Log($"{GetType().Name} effect not found: {popupType}");
                return null;
            }

            if (parent == null)
            {
                parent = _effectsContainer;
            }
            var effect = GetFromPool(popupType, prefab, position, parent);
            effect.OnComplete += ReturnToPool;
            if (spawn)
            {
                effect.Spawn();
            }
            return effect;
        }

        private async Task LoadPrefabAsync(PopupType popupType)
        {
            foreach (var effectAsset in _effectAssets)
            {
                if (effectAsset.popupType == popupType)
                {
                    
                    GameObject prefab = await effectAsset.effectPrefab.LoadAssetAsync<GameObject>().Task;
                    
                    _prefabCache[popupType] = prefab;
                    break;
                }
            }
        }

        private Popup GetFromPool(PopupType popupType, GameObject prefab, Vector3 position, Transform parent)
        {
            if (_objectPools.TryGetValue(popupType, out Queue<Popup> pool) && pool.Count > 0)
            {
                var effect = pool.Dequeue();
                effect.transform.position = position;
                effect.transform.SetParent(parent);
                return effect;
            }

            var newEffect = Instantiate(prefab, position, Quaternion.identity, parent).GetComponent<Popup>();
            newEffect.gameObject.SetActive(false);
            return newEffect;
        }

        private void ReturnToPool(Popup effect)
        {
            effect.OnComplete -= ReturnToPool;
            effect.gameObject.SetActive(false);
            
            if (!_objectPools.ContainsKey(effect.Type))
            {
                _objectPools[effect.Type] = new Queue<Popup>();
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
