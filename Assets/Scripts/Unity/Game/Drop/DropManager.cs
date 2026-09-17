using System.Collections.Generic;
using System.Linq;
using Core.Application.Models;
using DG.Tweening;
using Unity.Infrastructure.Effects;
using Unity.Presentation.HUD;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Zenject;

namespace Unity.Game
{
    
    [RequireComponent(typeof(SceneDropAnimator))]
    public class DropManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _sceneDropPrefab;
        [SerializeField]
        private SceneDropAnimator _sceneDropAnimator;
        [SerializeField]
        private Transform _sceneDropContainer;
        
        [SerializeField]
        private DropFlyToTargetAnimator _flyToTargetAnimator;
       
        
        [Inject] private IEnumerable<IDropTarget> _dropTargets;
        [Inject] private PopupSpawnManager _effectSpawnManager;
        [Inject] private DiContainer _container;
        
        private readonly List<Transform> _dropTransforms = new List<Transform>();
        
        private void Update()
        {
            if (Pointer.current.press.wasPressedThisFrame)
            {
                Vector2 inputPosition = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
                //ShowDrop( new Resource(ResourceType.Money, 1)  ,inputPosition);
                _effectSpawnManager.SpawnRandomHitBubble(inputPosition, transform);
                // _effectSpawnManager.SpawnRandomExplosion(inputPosition, transform);
            }
        }

        public void ShowSceneDrop(Resource resource, Vector2 position, Vector2 direction = default)
        {
            var drop = _container.InstantiatePrefabForComponent<ResourceSprite>(_sceneDropPrefab);
            drop.transform.position = position;
            drop.SetResourceType(resource.Type);
            var dropTransform = drop.transform;
            dropTransform.localScale = Vector3.one * .5f;
            var dropTarget = _dropTargets.FirstOrDefault( t => t.ResourceType == resource.Type);
            _dropTransforms.Add(dropTransform);
            _sceneDropAnimator.Show(dropTransform, direction, (target) =>
            {
                if (dropTransform == null)
                {
                    return;
                }
                _flyToTargetAnimator.FlyToIcon(dropTarget.GetTargetRect(), dropTransform, 1,(drop) =>
                {
                    if (drop == null)
                    {
                        return;
                    }
                    dropTarget.AddResource(resource);
                    _dropTransforms.Remove(drop);
                    Destroy(drop.gameObject);
                });
            });
            
        }
        
        public void Reset()
        {
            foreach (var target in _dropTargets)
            {
                target.Clear();
            }

            while (_dropTransforms.Count > 0)
            {
                var drop = _dropTransforms[0];
                _dropTransforms.RemoveAt(0);
                Destroy(drop.gameObject);
            }
        }
        
        
    }
}