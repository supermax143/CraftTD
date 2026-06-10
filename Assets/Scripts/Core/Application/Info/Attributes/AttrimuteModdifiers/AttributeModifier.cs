using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttrimuteModdifiers
{
    public abstract class AttributeModifierBase
    {
        public abstract GameEntityAttributeKind Kind { get; }
    }

    // Дженерик-наследник для типобезопасной работы с конкретным атрибутом
    public abstract class AttributeModifier<TValue> : AttributeModifierBase
    {
        public abstract TValue Apply(TValue baseValue);
    }
}