using Core.Application.Info.Attributes.AttrimuteModdifiers;
using Unity.Game;

namespace Core.Application.Models
{
    public class TowerModel
    {
        private readonly TowerEntityInfo _info;

        public TowerModel(TowerEntityInfo info)
        {
            _info = info;
        }

        public TowerEntityInfo Info => _info;

        public void AddModifier(HealthTowerAddModifier healthTowerAddModifier)
        {
            _info.AddModifier(healthTowerAddModifier);
        }
    }
}