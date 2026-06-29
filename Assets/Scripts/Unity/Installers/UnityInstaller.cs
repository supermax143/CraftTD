using Core.Application.DataStorage;
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
using Unity.Infrastructure.Tutorial;
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
      private TutorialController _tutorialController;
      [SerializeField]
      private GameSettings _gameSettings;
      [SerializeField]
      private ChronologyInfo _chronologyInfo;
      [SerializeField]
      private GameStats _baseGameStats;
      [SerializeField]
      private AdvertisementController _advertisementController;


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
         Container.BindInterfacesAndSelfTo<ResourceManager>().AsSingle();
         Container.BindInterfacesAndSelfTo<GameEventsBus>().AsSingle();
         Container.BindInterfacesAndSelfTo<GameSettings>().FromInstance(_gameSettings);
         Container.BindInterfacesAndSelfTo<DummyPurchasesController>().AsSingle();
         Container.BindInterfacesAndSelfTo<DummyAdvertisementAPI>().AsSingle();
         Container.BindInterfacesAndSelfTo<AdvertisementController>().FromInstance(_advertisementController).AsSingle();
         Container.Bind<AdvertisementDoubleReward>().AsTransient();
         
         //Data Storage
         Container.Bind<ILocalStorageProvider>().To<PlayerPrefsStorageProvider>().AsTransient();
         Container.Bind<IGlobalStorageProvider>().To<PlayerPrefsStorageProvider>().AsTransient();
         
         //Tutorial
         Container.BindInterfacesAndSelfTo<TutorialController>().FromInstance(_tutorialController);
         BindController<ModelToAtlasRenderer>();
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