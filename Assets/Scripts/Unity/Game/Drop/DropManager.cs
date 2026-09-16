using System.Collections.Generic;
using System.Linq;
using Core.Application.Models;
using DG.Tweening;
using Unity.Infrastructure.Effects;
using Unity.Presentation.HUD;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Unity.Game
{
    
    [RequireComponent(typeof(DropAnimator))]
    public class DropManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _rewardMoneyView;
        [SerializeField]
        private DropAnimator _dropAnimator;
        [SerializeField]
        private DropFlyToTargetAnimator _flyToTargetAnimator;
       
        
        [Inject] private IEnumerable<IDropTarget> _dropTargets;
        [Inject] private PopupSpawnManager _effectSpawnManager;
        
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

        public void ShowDrop(Resource resource, Vector2 position, Vector2 direction = default)
        {
            GameObject rewardView = Instantiate(_rewardMoneyView, position, Quaternion.identity);
            var drop = rewardView.transform;
            drop.localScale = Vector3.one * .5f;
            var targetIcon = _dropTargets.FirstOrDefault().GetTargetRect();
            _dropTransforms.Add(drop);
            _dropAnimator.Show(drop, direction, (target) =>
            {
                if (drop == null)
                {
                    return;
                }
                _flyToTargetAnimator.FlyToIcon(targetIcon, drop, 1,(drop) =>
                {
                    if (drop == null)
                    {
                        return;
                    }
                    _dropTargets.FirstOrDefault().AddResource(resource);
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