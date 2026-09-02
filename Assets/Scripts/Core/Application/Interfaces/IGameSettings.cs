using Unity.Game;
using UnityEngine;

namespace Unity.Settings
{
    public interface IGameSettings
    {
        float ExplosionAnimationTime { get; }
        float EpochChangeTime { get; }
        int MaxEquipedSpells { get; }
        bool TryGetFactionColor(Faction faction, out Color color);
    }
}