using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity.Infrastructure.Effects
{
    /// <summary>
    /// Хелпер для получения случайных типов взрывов
    /// </summary>
    public class ExplosionHelper : MonoBehaviour
    {
        [SerializeField]
        private VisualEffectType[] _explosionTypes;

        public VisualEffectType GetRandomExplosionType()
        {
            return _explosionTypes[Random.Range(0, _explosionTypes.Length)];
        }
    }
}
