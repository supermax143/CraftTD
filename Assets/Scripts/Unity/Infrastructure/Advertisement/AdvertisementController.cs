using System;
using System.Collections;
using System.Collections.Generic;
using Core.Application.DataStorage;
using Unity.Infrastructure.Advertisement.API;
using Unity.Infrastructure.Advertisement.Transactions;
using Unity.Infrastructure.Purchases;
using Unity.Utils.Time;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.Advertisement
{
    public class AdvertisementController : MonoBehaviour, IAdvertisementController
    {
        private const int COOLDOWN_TIME = 180;

        public event Action OnAdvertisementStartShow;

        [Inject] private DiContainer _container;
        [Inject] private IDataStorage _storage;
        [Inject] private IPurchasesController _purchasesController;
        [Inject] private IAdvertisementAPI _advertisementAPI;
        
        private readonly List<AdvertisementBase> _advertisements = new List<AdvertisementBase>();
        private readonly Dictionary<Type, List<Delegate>> _typeToListeners = new();

        private AdvertisementBase _curAdvertisement;

        private readonly Timer _cooldownTimer = new Timer(TimeType.Fixed);

        
        private bool _adsRemoved = false;
        
        public void Initialize()
        {
            if (_storage.Purchases.HasAnyPurchases())
            {
                RemoveAllAds();
            }
            else
            {
                _purchasesController.OnPurchaseComplete += (_) => RemoveAllAds();
            }
        }

        private void RemoveAllAds()
        {
            Debug.Log($"{this.GetType().Name} RemoveAllAds");
            _adsRemoved = true;
            _advertisementAPI.RemoveAllAds();
        }

        public void AddListener<TAdvertisement>(Action<TAdvertisement> listener) where TAdvertisement : AdvertisementBase
        {
            var type = typeof(TAdvertisement);
            
            if (!_typeToListeners.TryGetValue(type, out var listeners))
            {
                listeners = new List<Delegate>();
                _typeToListeners[type] = listeners;
            }

            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void RemoveListener<TAdvertisement>(Action<TAdvertisement> listener) where TAdvertisement : AdvertisementBase
        {
            var type = typeof(TAdvertisement);

            if (_typeToListeners.ContainsKey(type))
            {
                _typeToListeners[type].Remove(listener);
            }

            foreach (var tAdv in _typeToListeners.Keys)
            {
                Debug.Log($"listener for {tAdv.Name}: {_typeToListeners[type].Count}");
            }
            
        }

        private void OnTransactionCompleted(AdvertisementBase advertisement)
        {
            advertisement.Completed -= OnTransactionCompleted;
            if(_curAdvertisement != advertisement)
            {
                Debug.Log("advertisement incorrect");
                return;
            }

            _curAdvertisement = null;
            _advertisements.Remove(advertisement);
            if (_curAdvertisement == advertisement)
            {
                _curAdvertisement = null;
            }
            
            var type = advertisement.GetType();

            if (_typeToListeners.TryGetValue(type, out var listeners))
            {
                for (int i = 0; i < listeners.Count; i++)
                {
                    listeners[i].DynamicInvoke(advertisement);
                }
            }
            
            TryShowNextAdvertisement();
        }

        public void ShowFullscreen()
        {
            if (!_cooldownTimer.IsComplete)
            {
                return;
            }
            ShowAdvertisement(_container.Resolve<AdvertisementFullscreen>());
            _cooldownTimer.Start(COOLDOWN_TIME);
        }


        public void ShowRewardedContinueGame()
        {
            ShowAdvertisement(_container.Resolve<AdvertisementContinueGame>());
        }

        public void ShowRewardedAddReward()
        {
            ShowAdvertisement(_container.Resolve<AdvertisementDoubleReward>());
        }

        private void ShowAdvertisement(AdvertisementBase adv)
        {
            if (_adsRemoved && !adv.Rewarded)
            {
                return;
            }
            Debug.Log($"ShowAdvertisement: {adv.ToString()}");
            _advertisements.Add(adv);
            StartCoroutine(TryShowNextAdvertisement());
        }
        
        private IEnumerator TryShowNextAdvertisement()
        {
            if (_curAdvertisement != null)
            {
                yield break;
            }

            if (_advertisements.Count == 0)
            {
                yield break;
            }
            
            OnAdvertisementStartShow?.Invoke();
            yield return new WaitForEndOfFrame();
            _curAdvertisement = _advertisements[0];
            _curAdvertisement.Initialize();
            _advertisements.RemoveAt(0);
            _curAdvertisement.Completed += OnTransactionCompleted;
            Debug.Log($"Advertisement started: {_curAdvertisement.ToString()}");
            _curAdvertisement.Execute();
        }

        
        
    }
}
