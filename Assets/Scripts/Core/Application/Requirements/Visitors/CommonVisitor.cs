using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Infrastructure.Requirements.Base;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.Requirements.Visitors
{
    /// <summary>
    /// принимает на вход  любой рекваермент
    /// далее в зависимости от его типа ищет предсозданный чекер
    /// если нашел - прогоняет рекваермент через чекер
    /// </summary>
    [UsedImplicitly]
    public sealed class CommonVisitor : IRequirementVisitor
    {
        [Inject] private DiContainer _container;

        private readonly Dictionary<System.Type, IRequirementVisitor> _visitors = new();
        
        //для удобства использования рекваерментов
        //этот визитер используется как дефолтный если не был передан конкретный
        internal static IRequirementVisitor Instance { get; private set; }

        //по-хорошему сюда не контейнер, а фабрику нужно прокинуть
        /*public override void InitModel()
        {
            Instance = this;
            _visitors[typeof(ReqHoldResources)] = _container.Instantiate<ReqHoldResourcesVisitor>();
        }*/

        public bool Check<T>(T req) where T : class, IRequirement
        {
            if (_visitors.TryGetValue(req.GetType(), out var result))
                return result.Check(req);

            Debug.LogError($"Can't find registered visitor for type {typeof(T)}");
            return false;
        }
    }
}