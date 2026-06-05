using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unity.Game
{
    [CreateAssetMenu(menuName = "CraftTD/ChronologyData", order = 1)]
    public class ChronologyData : ScriptableObject
    {
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "_epochName")]
        [SerializeField]
        private List<EpochData> _epochs;

        public List<EpochData> Epochs => _epochs;

        public bool TryGetEpoch(int index, out EpochData epoch)
        {
            epoch = default;
            if (index >= 0 && index < _epochs.Count)
            {
                epoch = _epochs[index];
                return  true;
            }
            return false;
        }
    }
}