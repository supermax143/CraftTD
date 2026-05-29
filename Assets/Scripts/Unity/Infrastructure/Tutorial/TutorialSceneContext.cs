using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.Tutorial
{
    public class TutorialSceneContext : MonoBehaviour
    {
        [Inject] private TutorialController _tutorialController;
        [Inject] private DiContainer _container;
        
        private void Start()
        {
            _tutorialController.SetSceneContext(_container);
        }

    }
}