using System;
using Core.Application.Models;

namespace Unity.Infrastructure.Requirements
{
    [Serializable]
    public class ReqHoldItem : RequirementBase
    {
        [Serializable]
        public struct ItemData
        {
            public string Id;
            public int Count;
        }
        
        public ItemData[] Item;
    }
    
}