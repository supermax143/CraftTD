using UnityEngine;

namespace Unity.Game
{
    public class AttackTarget : MonoBehaviour
    {
        private Faction _faction;

        public Faction Faction => _faction;
        
        public void SetFaction(Faction faction)
        {
            _faction = faction;
        }
        
    }
}