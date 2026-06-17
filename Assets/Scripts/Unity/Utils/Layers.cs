using UnityEngine;

namespace Unity.Utils
{
    public static class Layers
    {
        public static readonly int Player = LayerMask.NameToLayer(nameof(Player));
        public static readonly int Enemy = LayerMask.NameToLayer(nameof(Enemy));
        public static readonly int PlayerTower = LayerMask.NameToLayer(nameof(PlayerTower));
        public static readonly int EnemyTower = LayerMask.NameToLayer(nameof(EnemyTower));
    }
}