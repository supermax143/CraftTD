using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ShowDrop(inputPosition);
            }
        }

        public void ShowDrop(Vector2 position)
        {
            GameObject rewardView = Instantiate(_rewardMoneyView, position, Quaternion.identity);
            var drop = rewardView.transform;
            drop.localScale = Vector3.one * .5f;

            _dropAnimator.Show(drop, (target) =>
            {
                _flyToTargetAnimator.FlyToIcon(drop, (drop) =>
                {
                    //Destroy(drop.gameObject);
                });
            });
            
        }
        
        
        
    }
}