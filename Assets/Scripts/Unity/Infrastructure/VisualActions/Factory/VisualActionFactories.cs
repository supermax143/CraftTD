using System;
using System.Collections.Generic;
using Unity.Infrastructure.VisualActions.Actions;
using Unity.Infrastructure.VisualActions.ActionsData;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.VisualActions.Factory
{
    public class VisualActionFactories : MonoBehaviour, IVisualActionFactories
    {
        [Inject] private readonly DiContainer _diContainer;

        private Dictionary<Type, Func<IActionData, VisualActionBase>> _factories;

        public Dictionary<Type, Func<IActionData, VisualActionBase>> GetFactories(GameObject go)
        {
            if (_factories != null)
            {
                return _factories;
            }

            VisualActionBase SpawnAction<T>(IActionData data) where T : VisualActionBase
            {
                var action = _diContainer.InstantiateComponent<T>(gameObject);
                action.Initialize(data);
                return action;
            }

            _factories = new Dictionary<Type, Func<IActionData, VisualActionBase>>
            {
                { typeof(ChangeEpochActionData), SpawnAction<ChangeEpochVisual> },
                { typeof(ShowResultActionData), SpawnAction<ShowResultVisual> },
            };

            return _factories;
        }
    }
}