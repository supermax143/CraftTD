using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application.Interfaces;
using Core.Application.Models;
using Unity.Game.Attributes;
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
      
      [Serializable]
      private struct AttributeIcon
      {
         public GameEntityAttributeKind attributeKind;
         public AssetReferenceSprite sprite;
      }
      
      [SerializeField]
      private ResourceIcon[] _resourceIcons;
      [SerializeField]
      private AttributeIcon[] _attributeIcons;
      
      
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
      
      
      public bool TryGetAttributeIcon(GameEntityAttributeKind attributeKind, out AssetReferenceSprite attributeAsset)
      {
         attributeAsset = default;
         foreach (var attrIcon in _attributeIcons)
         {
            if (attrIcon.attributeKind == attributeKind)
            {
               attributeAsset = attrIcon.sprite;
               return true;
            }
         }
         return false;
      }
      
      public Task<T> Load<T>(string key, string tag)
         where T : Object => AddressableExtention.Load<T>(key, tag);
   }
}