using CartoonFX;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.Camera
{
    using Camera = UnityEngine.Camera;

    public class CameraController : MonoBehaviour, ICameraController, IInitializable
    {
        [SerializeField]
        private CameraShaker _cameraShake;
        
        [SerializeField]
        private Camera _camera;

        public void Initialize()
        {
            _cameraShake.Initialize(_camera);
        }

        public void ShakeCamera()
        {
            _cameraShake.Shake();
        }

    }
}