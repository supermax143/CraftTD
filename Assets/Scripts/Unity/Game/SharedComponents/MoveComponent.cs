using UnityEngine;

namespace Unity.Game
{
    public class MoveComponent : MonoBehaviour
    {
        private MoveData _data;

        public MoveData Data => _data;

        public void Initialize(MoveData data)
        {
            _data = data;   
        }
    }
}