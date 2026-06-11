using Core.Application.Info.Attributes.AttrimuteModdifiers;
using Unity.Game;

namespace Core.Application.Models
{
    public class TowerModel
    {
        private readonly TowerEntityInfo _info;
        private readonly Faction _faction;

        public TowerModel(TowerEntityInfo info, GameStats gameStats, Faction faction, int epochId)
        {
            _info = info;
            _faction = faction;
            _info.Initialize(gameStats, (uint)epochId, faction);
        }

        public TowerEntityInfo Info => _info;

        public Faction Faction => _faction;

        public void AddModifier(AttributeModifierBase modifier)
        {
            _info.AddModifier(modifier);
        }
    }
}