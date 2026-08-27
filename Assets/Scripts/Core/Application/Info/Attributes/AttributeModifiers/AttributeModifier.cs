using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public abstract class AttributeModifierBase
    {
        
        public abstract GameEntityAttributeKind AttributeKind { get; }

        public abstract void Replace(AttributeModifierBase modifier);

    }

    // Дженерик-наследник для типобезопасной работы с конкретным атрибутом
    public abstract class AttributeModifier<TValue> : AttributeModifierBase
    {
        protected TValue _value;

        protected AttributeModifier(TValue value) : base()
        {
            _value = value;
        }

        public override void Replace(AttributeModifierBase modifier)
        {
            if (modifier is AttributeModifier<TValue> typedModifier)
            {
                _value = typedModifier._value;
            }
        }

        public abstract TValue Apply(TValue baseValue);
    }
}