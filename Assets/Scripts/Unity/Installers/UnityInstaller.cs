using Core.Application.DataStorage;
using Unity.Bootstrap;
using Unity.Infrastructure.Advertisement.API;
using Unity.Infrastructure.DataStorage;
using Unity.Infrastructure.GameEvents;
using Unity.Infrastructure.Localization;
using Unity.Infrastructure.Purchases;
using Unity.Infrastructure.ResourceManager;
using Unity.Infrastructure.Scenes;
using Unity.Infrastructure.Tutorial;
using Unity.Infrastructure.Windows;
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
      
     


      public override void InstallBindings()
      {
         InitializeAddressables();

         Container.BindInterfacesAndSelfTo<LocalizationController>().AsSingle();
         Container.BindInterfacesAndSelfTo<ScenesLoader>().AsSingle();
         Container.BindInterfacesAndSelfTo<WindowsController>().FromInstance(_windowsController);
         Container.BindInterfacesAndSelfTo<GameBootrstarp>().AsSingle();
         Container.BindInterfacesAndSelfTo<ResourceManager>().AsSingle();
         Container.BindInterfacesAndSelfTo<GameEventsBus>().AsSingle();
         Container.BindInterfacesAndSelfTo<DummyPurchasesController>().AsSingle();
         Container.BindInterfacesAndSelfTo<DummyAdvertisementAPI>().AsSingle();
         //Data Storage
         Container.Bind<ILocalStorageProvider>().To<PlayerPrefsStorageProvider>().AsTransient();
         Container.Bind<IGlobalStorageProvider>().To<PlayerPrefsStorageProvider>().AsTransient();
         
         //Tutorial
         Container.BindInterfacesAndSelfTo<TutorialController>().FromInstance(_tutorialController);
      }

      private static void InitializeAddressables()
      {
         var handleStorage = new HandleStorage();
         AsyncOpHandleExtension.Initialize(handleStorage);
         AddressableExtention.Initialize(handleStorage);
      }
      


   }
}