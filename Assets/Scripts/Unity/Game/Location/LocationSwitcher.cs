using System.Collections;
using UnityEngine;
using Utils.ColorEffects;
using Zenject;

namespace Unity.Game
{
    public class LocationSwitcher : MonoBehaviour
    {
        
        [SerializeField]
        private Transform _currentLocationPlaceholder;
        [SerializeField]
        private Transform _nextLocationPlaceholder;
        
        private FieldObjectEffectsController _effectController;
        private GameLocation _currentLocation;
        private GameLocation _nextLocation;

        [Inject] private DiContainer _container;

        public GameLocation CurrentLocation => _currentLocation;


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
            _nextLocation = _container.InstantiatePrefabForComponent<GameLocation>(location, _nextLocationPlaceholder);
            _nextLocation.Initialize();
            _nextLocation.transform.localPosition = Vector3.zero;
        }
        

        private void UpdateLocation()
        {
            if (_currentLocation != null)
            {
                Destroy(_currentLocation.gameObject);
            }
            
            _currentLocation = _nextLocation;
            _currentLocation.transform.parent = _currentLocationPlaceholder;
            _nextLocation = null;
            _effectController = _currentLocation.GetComponent<FieldObjectEffectsController>();
        }
        
    }
}