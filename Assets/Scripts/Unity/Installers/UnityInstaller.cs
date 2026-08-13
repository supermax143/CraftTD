using Core.Application.DataStorage;
using Core.Application.Info.Inventory;
using Core.Application.Info.Shop;
using Core.Application.Models;
using Exploration.Scripts.Controllers.ModelRender;
using Unity.Bootstrap;
using Unity.Game;
using Unity.Infrastructure.Advertisement;
using Unity.Infrastructure.Advertisement.API;
using Unity.Infrastructure.Advertisement.Transactions;
using Unity.Infrastructure.DataStorage;
using Unity.Infrastructure.GameEvents;
using Unity.Infrastructure.Localization;
using Unity.Infrastructure.Purchases;
using Unity.Infrastructure.ResourceManager;
using Unity.Infrastructure.Scenes;
using Unity.Infrastructure.Sound;
using Unity.Infrastructure.Tutorial;
using Unity.Infrastructure.VisualActions;
using Unity.Infrastructure.Windows;
using Unity.Settings;
using UnityEngine;
using Zenject;

namespace Unity.Installers
{
   internal class UnityInstaller : MonoInstaller
   {

      [SerializeField]
      private WindowsController _windowsController;
      [SerializeField]
      private GameSettings _gameSettings;
      [SerializeField]
      private ChronologyInfo _chronologyInfo;
      [SerializeField]
      private GameStats _baseGameStats;
      [SerializeField]
      private AdvertisementController _advertisementController;
      [SerializeField]
      private ShopConfig _shopConfig;
      [SerializeField]
      private InventoryConfig _inventoryConfig;
      

      public override async void InstallBindings()
      {
         InitializeAddressables();

         //Data
         Container.BindInterfacesAndSelfTo<ChronologyInfo>().FromNewScriptableObject(_chronologyInfo).AsSingle();
         Container.BindInterfacesAndSelfTo<GameStats>().FromInstance(_baseGameStats);
         
         Container.BindInterfacesAndSelfTo<LocalizationController>().AsSingle();
         Container.BindInterfacesAndSelfTo<ScenesLoader>().AsSingle();
         Container.BindInterfacesAndSelfTo<WindowsController>().FromInstance(_windowsController);
         Container.BindInterfacesAndSelfTo<GameBootrstarp>().AsSingle();
         Container.BindInterfacesAndSelfTo<GameEventsBus>().AsSingle();
         Container.BindInterfacesAndSelfTo<GameSettings>().FromInstance(_gameSettings);
         Container.BindInterfacesAndSelfTo<DummyPurchasesController>().AsSingle();
         Container.BindInterfacesAndSelfTo<DummyAdvertisementAPI>().AsSingle();
         Container.BindInterfacesAndSelfTo<AdvertisementController>().FromInstance(_advertisementController).AsSingle();
         Container.BindInterfacesAndSelfTo<ActionsDispatcher>().AsSingle();
         Container.Bind<AdvertisementDoubleReward>().AsTransient();
         
         //Inventory 
         Container.BindInterfacesAndSelfTo<InventoryModel>().AsSingle();
         Container.BindInterfacesAndSelfTo<InventoryConfig>().FromInstance(_inventoryConfig).AsSingle();
         
         //Shop
         Container.BindInterfacesAndSelfTo<ShopModel>().AsSingle();
         Container.BindInterfacesAndSelfTo<ShopConfig>().FromInstance(_shopConfig).AsSingle();
         
         //Data Storage
         Container.Bind<ILocalStorageProvider>().To<PlayerPrefsStorageProvider>().AsTransient();
         Container.Bind<IGlobalStorageProvider>().To<PlayerPrefsStorageProvider>().AsTransient();
         
         //Tutorial
         BindController<TutorialController>();
         BindController<ModelToAtlasRenderer>();
         BindController<ResourceManager>();
         BindController<SoundManager>();
      }

      private static void InitializeAddressables()
      {
         var handleStorage = new HandleStorage();
         AsyncOpHandleExtension.Initialize(handleStorage);
         AddressableExtention.Initialize(handleStorage);
      }
      
      private void BindController<TController>() where TController: Component
      {
         var instance = gameObject.GetComponentInChildren<TController>();
         Container.BindInterfacesAndSelfTo<TController>().FromInstance(instance);
      }

   }
}