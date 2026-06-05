using Core.Application.Interfaces;
using Core.Domain.Services.ApplicationSession;
using Zenject;

namespace Core.Application.ApplicationSession.States
{
   internal abstract class SessionStateBase : ISessionStateInternal
   {
      
      [Inject] protected ApplicationStateMachine ApplicationStateMachine;
      [Inject] protected IScenesLoader _scenesLoader;

      public void Enter()
      {
         OnStateEnter();
      }

      public void Exit()
      {
         OnStateExit();
      }

      public virtual void StartGame() { }
      public virtual void ExitGame() { }
      

      protected abstract void OnStateEnter();
      
      protected virtual void OnStateExit() 
      {
      }

   }
}