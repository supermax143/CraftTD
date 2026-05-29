using System;

namespace Core.Application.Interfaces
{
   public interface IBootstrapProgress
   {
      event Action OnInitializationComplete;
      string CurStepIdent { get; }
      float Progress { get; }
      event Action OnStepStarted;
   }
}