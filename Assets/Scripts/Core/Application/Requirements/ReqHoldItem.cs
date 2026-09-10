using System;

namespace Core.Application.Requirements
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