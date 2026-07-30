using System.Collections.Generic;
using System.Linq;
using Core.Application.Models;
using DG.Tweening;
using Unity.Infrastructure.Effects;
using Unity.Presentation.HUD;
using UnityEngine;
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
        [Inject] private VisualEffectSpawnManager _effectSpawnManager;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            _dropAnimator.Show(drop, direction, (target) =>
            {
                _flyToTargetAnimator.FlyToIcon(targetIcon, drop, 2,(drop) =>
                {
                    _dropTargets.FirstOrDefault().AddResource(resource);
                    Destroy(drop.gameObject);
                });
            });
            
        }
        
        
        
    }
}