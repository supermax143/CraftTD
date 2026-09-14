using System.Collections.Generic;
using Core.Application.Models.Quests;
using Core.Application.Requirements.Base;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Core.Application.Requirements.Checkers
{
  
    public sealed class CommonReqChecker : IRequirementChecker, IInitializable
    {
        [Inject] private DiContainer _container;

        private readonly Dictionary<System.Type, IRequirementChecker> _checkers = new();
        

        //по-хорошему сюда не контейнер, а фабрику нужно прокинуть
        public void Initialize()
        {
            _checkers[typeof(ReqHoldResources)] = _container.Instantiate<ReqHoldResourcesChecker>();
        }


        public bool Check<T>(T req) where T : class, IRequirement
        {
            if (_checkers.TryGetValue(req.GetType(), out var result))
                return result.Check(req);

            Debug.LogError($"Can't find registered checker for type {typeof(T)}");
            return false;
        }

        public float GetProgress<T>(T req) where T : class, IRequirement
        {
            throw new System.NotImplementedException();
        }
    }
}