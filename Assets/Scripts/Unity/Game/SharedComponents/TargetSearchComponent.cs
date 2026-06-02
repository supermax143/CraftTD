using UnityEngine;

namespace Unity.Game
{
    public class TargetSearchComponent : MonoBehaviour
    {
        private TargetSearchData _data;

        public void Initialize(TargetSearchData data)
        {
            _data = data;   
        }
        
    }
}