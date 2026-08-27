using System;
using Environments.Common.Scripts;
using UnityEngine;

namespace Unity.Game
{
    public class GameLocation : MonoBehaviour
    {
        public event Action<Vector2> OnRoadClick;
        
        [SerializeField]
        private GameField _road;

        public void Initialize()
        {
            _road.OnClick += RoadClickHandler;
            _road.Initialize();
        }    
        

        private void RoadClickHandler(ITouchTarget target, Vector2 position)
        {
            OnRoadClick?.Invoke(position);
        }

        public void ActivateRoadClick(bool active)
        {
            _road.SetAsClickTarget(active);
        }

        private void OnDestroy()
        {
            _road.OnClick -= RoadClickHandler;
        }
    }
}