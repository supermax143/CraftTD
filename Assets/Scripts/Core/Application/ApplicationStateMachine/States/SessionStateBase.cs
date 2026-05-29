using Core.Domain.Services.ApplicationSession;
using Zenject;

namespace Core.Application.ApplicationSession.States
{
   internal abstract class SessionStateBase : ISessionStateInternal
   {
      
      [Inject] protected ApplicationStateMachine ApplicationStateMachine;

      public void Enter()
      {
         OnStateEnter();
      }

      public void Exit()
      {
         OnStateExit();
      }

      public virtual void StartGame() { }


      protected abstract void OnStateEnter();
      
      protected virtual void OnStateExit() 
      {
      }

   }
}