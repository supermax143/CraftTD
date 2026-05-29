using Core.Application.ApplicationSession.States;
using Core.Application.Models;
using Zenject;

namespace Core.Application.Installers
{
   public class ApplicationInstaller  : MonoInstaller
   {
      public override void InstallBindings()
      {
         
         //Data storage
         Container.BindInterfacesAndSelfTo<DataStorage.DataStorage>().AsSingle().NonLazy();
         
         // Session
         Container.Bind<BootstrapState>().AsTransient();
         Container.Bind<MainMenuState>().AsTransient();
         Container.Bind<GameState>().AsTransient();
         Container.BindInterfacesAndSelfTo<ApplicationSession.ApplicationStateMachine>().AsSingle();
         
         
         //Models
         Container.BindInterfacesAndSelfTo<MainModel>().AsSingle();
         
      }
   }
}