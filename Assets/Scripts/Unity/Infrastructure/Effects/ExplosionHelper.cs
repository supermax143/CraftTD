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
        private PopupType[] _explosionTypes;

        public PopupType GetRandomExplosionType()
        {
            return _explosionTypes[Random.Range(0, _explosionTypes.Length)];
        }
    }
}
