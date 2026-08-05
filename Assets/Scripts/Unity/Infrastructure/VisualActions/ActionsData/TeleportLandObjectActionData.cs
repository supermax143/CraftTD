using UnityEngine;

namespace Unity.Infrastructure.VisualActions.ActionsData
{
    public class TeleportLandObjectActionData : IActionData
    {
        public Vector2Int Cell { get; private set; }
        public long Id { get; private set; }
        
        public TeleportLandObjectActionData(long id, Vector2Int cell)
        {
            Id = id;
            Cell = cell;
        }

    }
}