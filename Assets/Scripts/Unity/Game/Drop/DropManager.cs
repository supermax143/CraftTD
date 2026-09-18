using System;
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
        private GameObject _uiDropPrefab;
        [SerializeField]
        private UIDropAnimator _uiDropAnimator;
        [SerializeField]
        private Transform _uiDropContainer;

        [SerializeField]
        private DropFlyToTargetAnimator _flyToTargetAnimator;
       
        
        [Inject] private IEnumerable<IDropTarget> _dropTargets;
        [Inject] private PopupSpawnManager _effectSpawnManager;
        [Inject] private DiContainer _container;
        
        private readonly List<Transform> _dropTransforms = new List<Transform>();
        private readonly List<RectTransform> _uiDropTransforms = new List<RectTransform>();
        
        private void Update()
        {
            if (Pointer.current.press.wasPressedThisFrame)
            {
                Vector2 inputPosition = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
                // ShowUiDrop(new Resource(ResourceType.Money, 100)  ,inputPosition);
                // ShowSceneDrop(new Resource(ResourceType.Money, 100)  ,inputPosition);
                _effectSpawnManager.SpawnRandomHitBubble(inputPosition, transform);
                // _effectSpawnManager.SpawnRandomExplosion(inputPosition, transform);
            }
        }

        public void ShowSceneDrop(Resource resource, bool isTemp, Vector2 position, Vector2 direction = default,
            Action finishCallback = null)
        {
            var drop = _container.InstantiatePrefabForComponent<ResourceSprite>(_sceneDropPrefab, position, Quaternion.identity,_sceneDropContainer);
            drop.transform.position = position;
            drop.SetResourceType(resource.Type);
            var dropTransform = drop.transform;
            dropTransform.localScale = Vector3.one * .5f;
            var dropTarget = _dropTargets.FirstOrDefault( t => t.ResourceType == resource.Type && t.IsTemp == isTemp);
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
                    finishCallback?.Invoke();
                });
            });

        }

        public void ShowUiDrop(Resource resource, bool isTemp, Vector2 position, Action finishCallback = null)
        {
            
            int dropCount = Mathf.Max(1, Mathf.Min(resource.Value, 20));
            var dropTarget = _dropTargets.FirstOrDefault(t => t.ResourceType == resource.Type && t.IsTemp == isTemp);
            int baseResourcePerDrop = resource.Value / dropCount;
            int remainder = resource.Value % dropCount;
            int completedDrops = 0;

            for (int i = 0; i < dropCount; i++)
            {
                var drop = _container.InstantiatePrefabForComponent<ResourceImage>(_uiDropPrefab, _uiDropContainer);
                var rectTransform = drop.GetComponent<RectTransform>();

                Vector2 screenPosition = Camera.main.WorldToScreenPoint(position);

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _uiDropContainer as RectTransform,
                    screenPosition,
                    null,
                    out Vector2 localPosition);

                rectTransform.anchoredPosition = localPosition;
                drop.SetResourceType(resource.Type);

                _uiDropTransforms.Add(rectTransform);

                int resourceAmount = baseResourcePerDrop + (i < remainder ? 1 : 0);
                Resource resourcePerDrop = new Resource(resource.Type, resourceAmount);

                _uiDropAnimator.Show(rectTransform, (target) =>
                {
                    if (rectTransform == null)
                    {
                        return;
                    }
                    _flyToTargetAnimator.FlyUiToIcon(dropTarget.GetTargetRect(), rectTransform, 0, (dropTransform) =>
                    {
                        if (dropTransform == null)
                        {
                            return;
                        }
                        dropTarget.AddResource(resourcePerDrop);
                        _uiDropTransforms.Remove(rectTransform);
                        Destroy(dropTransform.gameObject);
                        completedDrops++;
                        if (completedDrops == dropCount)
                        {
                            finishCallback?.Invoke();
                        }
                    });
                });
            }
        }
        
        public void Reset()
        {
            foreach (var target in _dropTargets)
            {
                if (!target.IsTemp)
                {
                    continue;
                }
                target.Clear();
            }

            while (_dropTransforms.Count > 0)
            {
                var drop = _dropTransforms[0];
                _dropTransforms.RemoveAt(0);
                Destroy(drop.gameObject);
            }

            /*while (_uiDropTransforms.Count > 0)
            {
                var drop = _uiDropTransforms[0];
                _uiDropTransforms.RemoveAt(0);
                Destroy(drop.gameObject);
            }*/
        }
        
        
    }
}