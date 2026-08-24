# resource-entity Specification

## Purpose
TBD - created by archiving change introduce-resource-struct. Update Purpose after archive.

## ADDED Requirements

### Requirement: Spell unlock cost
The system SHALL support existing Resource types (Money or Crystall) for spell unlock cost.

#### Scenario: Unlock spell with Money cost
- **WHEN** spell has UnlockCost of 500 Money and sufficient resources are available
- **THEN** spell can be unlocked and 500 Money is deducted

#### Scenario: Unlock spell with Crystall cost
- **WHEN** spell has UnlockCost of 300 Crystall and sufficient resources are available
- **THEN** spell can be unlocked and 300 Crystall is deducted

### Requirement: Spell upgrade cost
The system SHALL support Resource cost for spell upgrade.

#### Scenario: Upgrade spell with Money cost
- **WHEN** spell upgrade has cost of 100 Money and sufficient resources are available
- **THEN** spell can be upgraded and 100 Money is deducted

#### Scenario: Upgrade spell with different cost per level
- **WHEN** spell level 2 costs 100 Money and level 3 costs 200 Money
- **THEN** correct cost is deducted for each level

### Requirement: Insufficient resources check
The system SHALL prevent unlock/upgrade when resources are insufficient.

#### Scenario: Insufficient Money for unlock
- **WHEN** spell has UnlockCost of 500 Money but player has only 300 Money
- **THEN** unlock fails and no resources are deducted

#### Scenario: Insufficient Money for upgrade
- **WHEN** spell upgrade costs 100 Money but player has only 50 Money
- **THEN** upgrade fails and no resources are deducted
