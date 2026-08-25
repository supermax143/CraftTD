using UnityEngine;

namespace Unity.Game
{
    public class GameLocation : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _roadSprite;
        
        public void ActivateRoadHighLight(bool active)
        {
            var color = active ? Color.white : Color.yellow;
            _roadSprite.color = color;
        }
    }
}