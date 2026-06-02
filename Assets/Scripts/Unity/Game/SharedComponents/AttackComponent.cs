using UnityEngine;

namespace Unity.Game
{
    public class AttackComponent : MonoBehaviour
    {
        private AttackData _data;

        public AttackData Data => _data;

        public void Initialize(AttackData data)
        {
            _data = data;
        }
        
        public bool CheckRange(AttackTarget target)
        {
            var position = transform.position;
            var targetPosition = target.GetClosestPosition(position);
            var distance = Vector3.Distance(position, targetPosition);
            return distance <= _data.Range;
        }
        
    }
}