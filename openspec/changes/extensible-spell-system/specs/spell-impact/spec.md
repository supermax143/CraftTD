# spell-impact Specification

## Purpose
Определяет применение эффектов к выбранным целям. Impact получает результат Targeting и применяет один или несколько Effect.

## ADDED Requirements

### Requirement: SpellImpact applies effects to targets
The system SHALL define SpellImpact that applies effects to targets from Targeting.

#### Scenario: Apply single effect
- **WHEN** SpellImpact contains one Effect and is executed on targets
- **THEN** the effect is applied to all targets

#### Scenario: Apply multiple effects
- **WHEN** SpellImpact contains multiple Effects and is executed on targets
- **THEN** all effects are applied to all targets in sequence

#### Scenario: Impact receives targeting result
- **WHEN** SpellImpact is executed
- **THEN** it receives the targeting result from Targeting system

### Requirement: SpellImpact effect composition
The system SHALL allow SpellImpact to compose multiple effects.

#### Scenario: Compose Damage and Freeze effects
- **WHEN** SpellImpact contains DamageEffect and FreezeEffect
- **THEN** both effects are applied to targets

#### Scenario: Compose Damage, Freeze, and Burn effects
- **WHEN** SpellImpact contains DamageEffect, FreezeEffect, and BurnEffect
- **THEN** all three effects are applied to targets

### Requirement: SpellImpact execution order
The system SHALL define execution order for multiple effects.

#### Scenario: Effects execute in defined order
- **WHEN** SpellImpact contains multiple effects
- **THEN** effects are applied in the order they are defined in SpellImpact

### Requirement: SpellImpact with no targets
The system SHALL handle case when Targeting returns no targets.

#### Scenario: No targets from targeting
- **WHEN** Targeting returns no targets and SpellImpact is executed
- **THEN** no effects are applied

### Requirement: SpellImpact configuration
The system SHALL allow SpellImpact configuration in SpellDefinition.

#### Scenario: SpellImpact in SpellDefinition
- **WHEN** SpellDefinition is created
- **THEN** it contains SpellImpact configuration with effects
