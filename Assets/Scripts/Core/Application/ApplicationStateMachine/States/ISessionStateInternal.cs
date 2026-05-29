using Core.Application.Interfaces.ApplicationSession;

namespace Core.Domain.Services.ApplicationSession
{
   internal interface ISessionStateInternal : ISessionState
   {
      void Enter();
      void Exit();
   }
}