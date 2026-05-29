using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Infrastructure.Tutorial
{
    public class TutorialTag : MonoBehaviour
    {
        private static readonly Dictionary<string, List<TutorialTag>> _staticCache = new();

        [SerializeField] 
        private string _ident;

        public string Ident => _ident;
        

        private void Start()
        {
            if (string.IsNullOrEmpty(_ident))
            {
                return;
            }

            if (!_staticCache.TryGetValue(_ident, out var set))
            {
                _staticCache[_ident] = set = new List<TutorialTag>();
            }

            set.Add(this);
        }

        private void OnDestroy()
        {
            if (string.IsNullOrEmpty(_ident))
            {
                return;
            }

            if (_staticCache.TryGetValue(_ident, out var set) &&
                set.Remove(this) && 
                set.Count <= 0)
            {
                _staticCache.Remove(_ident);
            }
        }

        public static IReadOnlyList<TutorialTag> GetByIdent(string ident)
        {
            if (_staticCache.TryGetValue(ident, out var result)) return result;

            return Array.Empty<TutorialTag>();
        }

        public void SetIdent(string value)
        {
            if (!string.IsNullOrEmpty(_ident))
                return;

            _ident = value;
        }
    }
}