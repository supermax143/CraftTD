namespace Core.Application.Interfaces.ApplicationSession
{
   public interface IApplicationSession
   {
      ISessionState CurrentState { get; }
   }
}