using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
   public interface IScenesLoader
   {
      Task  LoadInitGameScene();
      Task  LoadMainMenuScene();
      Task  LoadGameScene();
      string CurScene { get; }
   }
}