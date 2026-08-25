using System;
using UnityEngine;

namespace Unity.Game
{
    public class GameLocation : MonoBehaviour
    {
        public event Action<Vector2> OnRoadClick;
        
        [SerializeField]
        private LocationRoad _road;

        private void Start()
        {
            _road.OnClick += RoadClickHandler;
        }

        private void RoadClickHandler(Vector2 position)
        {
            OnRoadClick?.Invoke(position);
        }

        public void ActivateRoadHighLight(bool active)
        {
            _road.ActivateHighLight(active);
        }

        private void OnDestroy()
        {
            _road.OnClick -= RoadClickHandler;
        }
    }
}