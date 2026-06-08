using System.Collections.Generic;
using Core.Application.Interfaces.Info;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unity.Game
{
    [CreateAssetMenu(menuName = "CraftTD/ChronologyData", order = 1)]
    public class ChronologyData : ScriptableObject, IChronologyInfo
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
        
        public bool TryGetEpochInfo(int index, out IEpochInfo epoch)
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