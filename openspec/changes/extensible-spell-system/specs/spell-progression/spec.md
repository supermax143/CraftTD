# spell-progression Specification

## Purpose
Определяет прогресс игрока для спеллов: IsUnlocked, Level, UnlockCost, UpgradeCost. Разделяет конфигурацию и runtime состояние.

## ADDED Requirements

### Requirement: SpellModel runtime state
The system SHALL define SpellModel containing player progression state for a spell.

#### Scenario: SpellModel contains current level
- **WHEN** SpellModel is created
- **THEN** it contains CurrentLevel field

#### Scenario: SpellModel contains unlock status
- **WHEN** SpellModel is created
- **THEN** it contains IsUnlocked field

#### Scenario: SpellModel references SpellDefinition
- **WHEN** SpellModel is created
- **THEN** it references the corresponding SpellDefinition

### Requirement: SpellModel calculated stats
The system SHALL calculate runtime spell stats from base configuration and level modifiers.

#### Scenario: Calculate stats at level 1
- **WHEN** SpellModel is at level 1
- **THEN** calculated stats equal base configuration

#### Scenario: Calculate stats with level modifiers
- **WHEN** SpellModel is at level 3 with damage +50 modifier
- **THEN** calculated damage equals base damage + 50

#### Scenario: Recalculate stats on level change
- **WHEN** SpellModel level changes from 2 to 3
- **THEN** calculated stats are recalculated with new level modifiers

### Requirement: SpellModel unlock
The system SHALL support unlocking spells through SpellModel.

#### Scenario: Unlock spell with sufficient resources
- **WHEN** SpellModel.Unlock() is called with sufficient resources
- **THEN** IsUnlocked becomes true and resources are deducted

#### Scenario: Unlock spell with insufficient resources
- **WHEN** SpellModel.Unlock() is called with insufficient resources
- **THEN** unlock fails and resources are not deducted

#### Scenario: Unlock already unlocked spell
- **WHEN** SpellModel.Unlock() is called on already unlocked spell
- **THEN** operation fails without resource deduction

### Requirement: SpellModel upgrade
The system SHALL support upgrading spell level through SpellModel.

#### Scenario: Upgrade spell with sufficient resources
- **WHEN** SpellModel.Upgrade() is called with sufficient resources
- **THEN** level increases by 1 and resources are deducted

#### Scenario: Upgrade spell to MaxLevel
- **WHEN** SpellModel.Upgrade() is called at MaxLevel
- **THEN** upgrade fails

#### Scenario: Upgrade spell with insufficient resources
- **WHEN** SpellModel.Upgrade() is called with insufficient resources
- **THEN** upgrade fails and resources are not deducted

### Requirement: SpellModel events
The system SHALL provide events for spell progression changes.

#### Scenario: OnSpellUnlocked event
- **WHEN** spell is unlocked
- **THEN** OnSpellUnlocked event is invoked

#### Scenario: OnSpellLevelChanged event
- **WHEN** spell level changes
- **THEN** OnSpellLevelChanged event is invoked with new level

### Requirement: SpellCollection management
The system SHALL provide SpellCollection for managing multiple SpellModels.

#### Scenario: Get spell by Id
- **WHEN** SpellCollection.GetSpell("fireball") is called
- **THEN** it returns the SpellModel for spell with Id "fireball"

#### Scenario: Get all spells
- **WHEN** SpellCollection.GetAllSpells() is called
- **THEN** it returns all SpellModels in the collection

#### Scenario: Unlock spell through collection
- **WHEN** SpellCollection.UnlockSpell("fireball") is called
- **THEN** the corresponding SpellModel is unlocked

#### Scenario: Upgrade spell through collection
- **WHEN** SpellCollection.UpgradeSpell("fireball") is called
- **THEN** the corresponding SpellModel level is increased
