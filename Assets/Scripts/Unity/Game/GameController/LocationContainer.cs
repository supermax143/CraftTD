using System.Collections;
using UnityEngine;
using Utils.ColorEffects;

namespace Unity.Game
{
    public class LocationContainer : MonoBehaviour
    {
        private FieldObjectEffectsController _effectController;
        private GameObject _currentLocation;
        
        public void Initialize(GameObject location)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            UpdateLocation(location);
        }
        
        public void SwitchToLocation(GameObject location, float duration)
        {
            StartCoroutine(AnimateLocationSwitch(location, duration));
        }

        private IEnumerator AnimateLocationSwitch(GameObject location, float duration)
        {
            yield return _effectController.ShowHorizontalDissolveEffect(duration, 1);
            UpdateLocation(location);
        }
        

        private void UpdateLocation(GameObject locationPrefab)
        {
            
            if (_currentLocation != null)
            {
                Destroy(_currentLocation);
            }
            
            _currentLocation = Instantiate(locationPrefab, transform);
            _currentLocation.transform.localPosition = Vector3.zero;
            _effectController = _currentLocation.GetComponent<FieldObjectEffectsController>();
        }
        
    }
}