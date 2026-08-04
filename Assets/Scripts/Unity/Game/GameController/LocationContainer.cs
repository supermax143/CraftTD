using System.Collections;
using UnityEngine;
using Utils.ColorEffects;

namespace Unity.Game
{
    public class LocationContainer : MonoBehaviour
    {
        
        [SerializeField]
        private Transform _currentLocationPlaceholder;
        [SerializeField]
        private Transform _nextLocationPlaceholder;
        
        private FieldObjectEffectsController _effectController;
        private GameObject _currentLocation;
        private GameObject _nextLocation;
        
        public void Initialize(GameObject location)
        {
            for (int i = _currentLocationPlaceholder.childCount - 1; i >= 0; i--)
            {
                Destroy(_currentLocationPlaceholder.GetChild(i).gameObject);
            }
            
            for (int i = _nextLocationPlaceholder.childCount - 1; i >= 0; i--)
            {
                Destroy(_nextLocationPlaceholder.GetChild(i).gameObject);
            }
            AddNextLocation(location);
            UpdateLocation();
        }
        
        public void SwitchToLocation(GameObject location, float duration, bool inversed)
        {
            StartCoroutine(AnimateLocationSwitch(location, duration, inversed));
        }

        private IEnumerator AnimateLocationSwitch(GameObject location, float duration, bool inversed = false)
        {
            AddNextLocation(location);
            yield return _effectController.ShowHorizontalDissolveEffect(duration, 1, inversed);
            UpdateLocation();
        }
        
        private void AddNextLocation(GameObject location)
        {
            _nextLocation = Instantiate(location, _nextLocationPlaceholder);
            _nextLocation.transform.localPosition = Vector3.zero;
        }
        

        private void UpdateLocation()
        {
            
            if (_currentLocation != null)
            {
                Destroy(_currentLocation);
            }
            
            _currentLocation = _nextLocation;
            _currentLocation.transform.parent = _currentLocationPlaceholder;
            _nextLocation = null;
            _effectController = _currentLocation.GetComponent<FieldObjectEffectsController>();
        }
        
    }
}