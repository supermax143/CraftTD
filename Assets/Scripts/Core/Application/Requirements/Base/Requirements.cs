using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Application.Requirements.Base
{
    /// <summary>
    /// API для работы с коллекцией рекваерментов
    /// </summary>
    [Serializable]
    public sealed class Requirements : IRequirement
    {
        [SerializeReference, SubclassSelector]
        private List<IRequirement> _requirements;

        public IReadOnlyList<IRequirement> Each => _requirements;

        public Requirements(int cap = 4) => _requirements = new List<IRequirement>(cap);

        public Requirements(List<IRequirement> requirements) 
            => _requirements = requirements;

        public bool Check(IRequirementChecker checker)
        {
            return _requirements.All(checker.Check);
        }

        public float GetProgress(IRequirementChecker checker)
        {
            return  _requirements.Sum(checker.GetProgress) / _requirements.Count;
        }
    }
}