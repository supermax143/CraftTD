using Core.Application.Spells.Targeting;
using UnityEngine;

namespace Unity.Game.Spells
{
    public abstract class SpellComponent
    {
        [SerializeField] private SpellTargetingComponent _targetingComponent;
        
        
    }
}