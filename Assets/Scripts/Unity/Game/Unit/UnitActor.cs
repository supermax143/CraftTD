using Unity.Game.Behavior;
using UnityEngine;

namespace Unity.Game
{
    public class UnitActor : MonoBehaviour
    {
        [SerializeField] 
        private float _health;
        [SerializeField] 
        private UnitAttack _attack;
        [SerializeField] 
        private Faction _faction;
        [SerializeField] 
        private UnitBehaviorBase _behavior;
        
        public float Health => _health;
        public UnitAttack Attack => _attack;
        public Faction Faction => _faction;
        public UnitBehaviorBase Behavior => _behavior;
    }
}