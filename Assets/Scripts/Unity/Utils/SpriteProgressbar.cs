using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Unity.Utils
{
    public class SpriteProgressbar : MonoBehaviour
    {
        [SerializeField]
        private Transform _background;
        [SerializeField]
        private Transform _progress;

        private float _maxValue;
        private float _currentValue;

        public void Initialize(float maxValue, float currentValue)
        {
            _maxValue = maxValue;
            _currentValue = currentValue;
            _progress.localScale = _background.localScale;
            UpdateView();
        }

        public void SetValue(float value)
        {
            _currentValue = value;
            UpdateView();
        }
        
        private void UpdateView()
        {
            if (_progress == null)
            {
                return;
            }
            _progress.localScale = new Vector3(_currentValue / _maxValue, 1, 1);
        }
    }
}