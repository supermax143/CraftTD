using UnityEngine;

namespace Unity.Game
{
    public class TowerView : MonoBehaviour
    {
        
        [SerializeField]
        private TintController _tintController;
        
        public void SetColor(Color color)
        {
            _tintController.SetTintColor(color);
        }
    }
}