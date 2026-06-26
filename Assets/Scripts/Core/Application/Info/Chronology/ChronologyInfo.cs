using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game
{
    [CreateAssetMenu(menuName = "CraftTD/ChronologyData", order = 1)]
    public class ChronologyInfo : ScriptableObject
    {
        [SerializeField]
        private List<EpochInfo> _epochs;

        public List<EpochInfo> Epochs => _epochs;

        public bool TryGetEpoch(int index, out EpochInfo epoch)
        {
            epoch = default;
            if (index >= 0 && index < _epochs.Count)
            {
                epoch = _epochs[index];
                return  true;
            }
            return false;
        }
        
        public bool TryGetEpochInfo(int index, out EpochInfo epoch)
        {
            epoch = default;
            if (TryGetEpoch(index, out var data))
            {
                epoch = data;
                return true;
            }
            return false;
        }


    }
}
