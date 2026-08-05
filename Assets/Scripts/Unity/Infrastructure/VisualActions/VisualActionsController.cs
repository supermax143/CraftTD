using System;
using System.Collections.Generic;
using ModestTree;
using Unity.Infrastructure.VisualActions.Actions;
using Unity.Infrastructure.VisualActions.ActionsData;
using Unity.Infrastructure.VisualActions.Factory;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.VisualActions
{
    public class VisualActionsController : MonoBehaviour, IVisualActionsController
    {
        [Inject] private readonly IVisualActionFactories _visualActionFactories;
        [Inject] private readonly IActionsDispatcher _actionsDispatcher;
        
        private readonly Queue<VisualActionBase> _actions = new();
        private Dictionary<Type, Func<IActionData, VisualActionBase>> _factories;
        private VisualActionBase _curAction;


        private void Start()
        {
            _factories = _visualActionFactories.GetFactories(gameObject);
            _actionsDispatcher.OnActionAdded += AddAction;
        }

        private bool TryCreateAction(IActionData actionData, out VisualActionBase action)
        {
            action = default;
            
            if (!_factories.TryGetValue(actionData.GetType(), out var factoryMethod))
                return false;

            action = factoryMethod(actionData);
            return true;
        }
        
        public void AddAction(IActionData actionData)
        {
            if (!TryCreateAction(actionData, out var action))
            {
                Debug.LogError($"no action for {actionData.GetType()}");
                return;
            }
            _actions.Enqueue(action);
            TryStartNextAction();
        }

        private void TryStartNextAction()
        {
            if (_actions.IsEmpty() || _curAction != null)
            {
                return;
            }

            _curAction = _actions.Dequeue();;
            _curAction.OnComplete += OnActionComplete;
            _curAction.Execute();
        }

        private void OnActionComplete(VisualActionBase action)
        {
            if (_curAction != action)
            {
                Debug.LogError("wrong action completed");
            }
            
            _curAction.OnComplete -= OnActionComplete;
            _curAction = null;
            TryStartNextAction();
        }

        private void OnDestroy()
        {
            _actionsDispatcher.OnActionAdded -= AddAction;
        }
    }
}