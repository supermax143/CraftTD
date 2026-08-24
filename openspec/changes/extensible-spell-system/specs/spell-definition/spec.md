# spell-definition Specification

## Purpose
Определяет конфигурацию спелла через ScriptableObject, содержащую immutable данные: Id, Name, Icon, Base configuration, MaxLevel, Upgrade configuration.

## ADDED Requirements

### Requirement: SpellDefinition ScriptableObject
The system SHALL define SpellDefinition as ScriptableObject containing immutable spell configuration.

#### Scenario: Create SpellDefinition with basic fields
- **WHEN** SpellDefinition is created
- **THEN** it contains Id, Name, Description, Icon, MaxLevel fields

#### Scenario: SpellDefinition contains base configuration
- **WHEN** SpellDefinition is created
- **THEN** it contains base configuration for Activation, Targeting, Impact

#### Scenario: SpellDefinition is not mutated at runtime
- **WHEN** spell level is upgraded
- **THEN** SpellDefinition base values remain unchanged

### Requirement: SpellDefinition Activation configuration
The system SHALL allow SpellDefinition to specify Activation type and parameters.

#### Scenario: SpellDefinition with Instant activation
- **WHEN** SpellDefinition has ActivationType Instant
- **THEN** it contains activation parameters for Instant type

#### Scenario: SpellDefinition with CastTime activation
- **WHEN** SpellDefinition has ActivationType CastTime
- **THEN** it contains castTime parameter

#### Scenario: SpellDefinition with Channel activation
- **WHEN** SpellDefinition has ActivationType Channel
- **THEN** it contains channelDuration parameter

### Requirement: SpellDefinition Targeting configuration
The system SHALL allow SpellDefinition to specify Targeting type and parameters.

#### Scenario: SpellDefinition with Unit targeting
- **WHEN** SpellDefinition has TargetingType Unit
- **THEN** it contains targeting parameters for Unit selection

#### Scenario: SpellDefinition with Field targeting
- **WHEN** SpellDefinition has TargetingType Field
- **THEN** it contains radius parameter for area selection

#### Scenario: SpellDefinition with Direction targeting
- **WHEN** SpellDefinition has TargetingType Direction
- **THEN** it contains direction and range parameters

### Requirement: SpellDefinition Impact configuration
The system SHALL allow SpellDefinition to specify Impact with one or more Effects.

#### Scenario: SpellDefinition with single effect
- **WHEN** SpellDefinition Impact contains one Effect
- **THEN** the effect is applied to targets

#### Scenario: SpellDefinition with multiple effects
- **WHEN** SpellDefinition Impact contains multiple Effects
- **THEN** all effects are applied to targets in sequence

### Requirement: SpellDefinition Upgrade configuration
The system SHALL allow SpellDefinition to specify upgrade configuration per level.

#### Scenario: SpellDefinition with upgrade levels
- **WHEN** SpellDefinition has MaxLevel 5
- **THEN** it contains upgrade configuration for levels 1-5

#### Scenario: Upgrade configuration contains cost
- **WHEN** upgrade configuration is defined for level
- **THEN** it contains cost in resources

#### Scenario: Upgrade configuration contains modifiers
- **WHEN** upgrade configuration is defined for level
- **THEN** it contains array of AttributeModifiers

### Requirement: SpellDefinition MaxLevel
The system SHALL define MaxLevel in SpellDefinition to limit maximum spell level.

#### Scenario: SpellDefinition with MaxLevel
- **WHEN** SpellDefinition has MaxLevel 5
- **THEN** spell cannot be upgraded beyond level 5

#### Scenario: SpellDefinition with different MaxLevel
- **WHEN** SpellDefinition has MaxLevel 10
- **THEN** spell can be upgraded up to level 10

### Requirement: SpellDatabase
The system SHALL provide SpellDatabase for managing multiple SpellDefinitions.

#### Scenario: SpellDatabase contains all spells
- **WHEN** SpellDatabase is loaded
- **THEN** it contains all SpellDefinitions in the project

#### Scenario: Get SpellDefinition by Id
- **WHEN** SpellDatabase.GetSpellDefinition("fireball") is called
- **THEN** it returns the SpellDefinition with Id "fireball"
