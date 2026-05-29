using System.Threading.Tasks;
using UnityEngine;

namespace Core.Application.Interfaces
{
   public interface IResourceManager
   {
      Task<T> Load<T>(string key, string tag)
         where T : Object;
   }
}