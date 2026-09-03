using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Application.Info.Attributes.AttributeModifiers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.Components.Containers
{
    /// <summary>
    /// Контейнер для отображения списка модификаторов атрибутов
    /// </summary>
    public class AttributesModifiersList : MonoBehaviour
    {
        [SerializeField]
        private Transform _parent;
        [SerializeField]
        private AttributeModifierItem _prefab;

        private readonly List<AttributeModifierItem> _items = new List<AttributeModifierItem>();

        [Inject] private DiContainer _diContainer;

        public async UniTask SetModifiers(List<AttributeModifierBase> modifiers)
        {
            ClearList();

            if (modifiers == null)
            {
                return;
            }

            foreach (var modifier in modifiers)
            {
                var item = _diContainer.InstantiatePrefabForComponent<AttributeModifierItem>(_prefab, _parent);
                await item.SetModifier(modifier);
                _items.Add(item);
            }
        }

        private void ClearList()
        {
            foreach (var item in _items)
            {
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }
            _items.Clear();
        }

        private void OnDestroy()
        {
            ClearList();
        }
    }
}
