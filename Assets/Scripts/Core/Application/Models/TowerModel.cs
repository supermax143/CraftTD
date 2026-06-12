using Core.Application.Info.Attributes.AttrimuteModdifiers;
using Unity.Game;
using Unity.Game.Entity;

namespace Core.Application.Models
{
    public class TowerModel
    {
        private readonly TowerInfo _info;
        private readonly Faction _faction;
        private readonly TowerEntity _entity;
        
        
        public TowerInfo Info => _info;
        public Faction Faction => _entity.Faction;

        public TowerModel(TowerInfo info, TowerEntity entity)
        {
            _info = info;
            _entity = entity;
        }


        public void AddModifier(AttributeModifierBase modifier)
        {
            _info.AddModifier(modifier);
        }
    }
}