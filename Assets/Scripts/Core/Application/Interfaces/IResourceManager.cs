using System.Threading.Tasks;
using Core.Application.Models;
using Unity.Game.Attributes;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Application.Interfaces
{
   public interface IResourceManager
   {
      Task<T> Load<T>(string key, string tag)
         where T : Object;

      bool TryGetResourceIcon(ResourceType resourceType, out AssetReferenceSprite resourceAsset);
      bool TryGetAttributeIcon(GameEntityAttributeKind attributeKind, out AssetReferenceSprite attributeAsset);
   }
}