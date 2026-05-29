using System;
using Core.Application.ApplicationSession.States;
using Core.Application.ApplicationStateMachine.States;
using Core.Application.Interfaces.ApplicationSession;
using Core.Domain.Services.ApplicationSession;
using Zenject;

namespace Core.Application.ApplicationSession
{
   internal class ApplicationStateMachine : IApplicationSession, IInitializable
   {

      public event Action<ISessionState> OnStateChanged;
      
      [Inject] private DiContainer _container;

      private ISessionStateInternal _currentState;

      public ISessionState CurrentState => _currentState;

      public void Initialize()
      {
#if DEBUG_MODE
         _currentState = _container.Resolve<DebugState>();
         return;
#endif
         ChangeState<BootstrapState>();
      }

      internal void ChangeState<TState>() where TState : ISessionStateInternal
      {
         var newState = _container.Resolve<TState>();
         ChangeState(newState);
      }
      
      private void ChangeState(ISessionStateInternal newState)
      {
         if (_currentState != null)
         {
            _currentState.Exit();
         }
            
         _currentState = newState;
         _currentState.Enter();
            
         OnStateChanged?.Invoke(_currentState);
      }

     
   }
}