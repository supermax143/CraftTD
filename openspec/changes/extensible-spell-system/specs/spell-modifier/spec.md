# spell-modifier Specification

## Purpose
Определяет интеграцию с существующей системой AttributeModifier для модификации параметров спеллов.

## ADDED Requirements

### Requirement: Spell parameters are modifiable
The system SHALL allow spell parameters to be modified through AttributeModifier system.

#### Scenario: Damage parameter modification
- **WHEN** AttributeModifier with Multiply 1.2 is applied to spell damage
- **THEN** spell damage is multiplied by 1.2

#### Scenario: Duration parameter modification
- **WHEN** AttributeModifier with Add 1 is applied to spell duration
- **THEN** spell duration increases by 1 second

#### Scenario: Radius parameter modification
- **WHEN** AttributeModifier with Override 5 is applied to spell radius
- **THEN** spell radius is set to 5

### Requirement: Supported spell parameters
The system SHALL support modification of spell parameters: Damage, HealAmount, Duration, Radius, Range, Cooldown, CastTime, EffectStrength, TickInterval.

#### Scenario: Modify Damage
- **WHEN** AttributeModifier is applied to Damage parameter
- **THEN** damage value is modified

#### Scenario: Modify HealAmount
- **WHEN** AttributeModifier is applied to HealAmount parameter
- **THEN** healAmount value is modified

#### Scenario: Modify Duration
- **WHEN** AttributeModifier is applied to Duration parameter
- **THEN** duration value is modified

#### Scenario: Modify Radius
- **WHEN** AttributeModifier is applied to Radius parameter
- **THEN** radius value is modified

#### Scenario: Modify Range
- **WHEN** AttributeModifier is applied to Range parameter
- **THEN** range value is modified

#### Scenario: Modify Cooldown
- **WHEN** AttributeModifier is applied to Cooldown parameter
- **THEN** cooldown value is modified

#### Scenario: Modify CastTime
- **WHEN** AttributeModifier is applied to CastTime parameter
- **THEN** castTime value is modified

#### Scenario: Modify EffectStrength
- **WHEN** AttributeModifier is applied to EffectStrength parameter
- **THEN** effectStrength value is modified

#### Scenario: Modify TickInterval
- **WHEN** AttributeModifier is applied to TickInterval parameter
- **THEN** tickInterval value is modified

### Requirement: Modifier types support
The system SHALL support existing AttributeModifier types: Add, Multiply, Override.

#### Scenario: Add modifier on spell parameter
- **WHEN** Add modifier with value 20 is applied to base damage 100
- **THEN** resulting damage is 120

#### Scenario: Multiply modifier on spell parameter
- **WHEN** Multiply modifier with value 1.2 is applied to base damage 100
- **THEN** resulting damage is 120

#### Scenario: Override modifier on spell parameter
- **WHEN** Override modifier with value 150 is applied to base damage 100
- **THEN** resulting damage is 150

### Requirement: Level modifiers
The system SHALL apply level modifiers from SpellUpgrade configuration to spell parameters.

#### Scenario: Apply level 2 modifiers
- **WHEN** spell is at level 2 with Damage +25 modifier
- **THEN** calculated damage equals base damage + 25

#### Scenario: Apply level 3 modifiers
- **WHEN** spell is at level 3 with Damage +50 and Radius +0.5 modifiers
- **THEN** calculated damage equals base damage + 50 and radius equals base radius + 0.5

#### Scenario: Apply multiple level modifiers
- **WHEN** spell is at level 5 with multiple modifiers
- **THEN** all modifiers are applied to respective parameters

### Requirement: Calculated runtime values
The system SHALL calculate runtime spell values from base configuration and level modifiers.

#### Scenario: Calculate runtime damage
- **WHEN** spell base damage is 100 and level adds +50
- **THEN** runtime damage is 150

#### Scenario: Calculate runtime duration
- **WHEN** spell base duration is 3 and level adds +1
- **THEN** runtime duration is 4

#### Scenario: Recalculate on level change
- **WHEN** spell level changes
- **THEN** runtime values are recalculated with new level modifiers

### Requirement: No ScriptableObject mutation
The system SHALL not mutate SpellDefinition ScriptableObject when applying modifiers.

#### Scenario: Modifiers don't mutate base values
- **WHEN** level modifiers are applied
- **THEN** SpellDefinition base values remain unchanged

#### Scenario: Runtime values are separate
- **WHEN** spell is upgraded
- **THEN** calculated runtime values are separate from SpellDefinition base values
