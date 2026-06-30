using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Application.Interfaces;
using Core.Application.Models;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Unity.Infrastructure.ResourceManager
{
   public class ResourceManager : MonoBehaviour, IResourceManager
   {
      [Serializable]
      private struct ResourceIcon
      {
         public ResourceType resourceType;
         public AssetReferenceSprite sprite;
      }
      
      [SerializeField]
      private ResourceIcon[] _resourceIcons;
      
      public bool TryGetResourceIcon(ResourceType resourceType, out AssetReferenceSprite resourceAsset)
      {
         resourceAsset = default;
         foreach (var resIcon in _resourceIcons)
         {
            if (resIcon.resourceType == resourceType)
            {
               resourceAsset = resIcon.sprite;
               return true;
            }
         }
         return false;
      }
      
      
      public Task<T> Load<T>(string key, string tag)
         where T : Object => AddressableExtention.Load<T>(key, tag);
   }
}