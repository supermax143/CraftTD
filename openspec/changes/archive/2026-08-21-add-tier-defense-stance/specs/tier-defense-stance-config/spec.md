## ADDED Requirements

### Requirement: Defense stance purchase through EpochModel
The system SHALL allow purchasing defense stance ability for a tier through EpochModel.OpenDefenseStance method (similar to OpenUnit).

#### Scenario: Purchase defense stance for tier 1
- **WHEN** player calls EpochModel.OpenDefenseStance(UnitTier.Tier1)
- **AND** player has enough crystals
- **AND** tier 1 units are unlocked
- **THEN** crystals are deducted from inventory
- **AND** defense stance is marked as purchased for tier 1
- **AND** OnDefenseStanceOpened event is invoked

#### Scenario: Purchase defense stance for tier 2
- **WHEN** player calls EpochModel.OpenDefenseStance(UnitTier.Tier2)
- **AND** player has enough crystals
- **AND** tier 2 units are unlocked
- **THEN** crystals are deducted from inventory
- **AND** defense stance is marked as purchased for tier 2

### Requirement: Defense stance cost calculation
The system SHALL calculate defense stance cost through GameStats.GetDefenseStanceCost based on epoch and tier.

#### Scenario: Get cost for tier 1 in epoch 1
- **WHEN** GameStats.GetDefenseStanceCost is called with epoch 1 and tier 1
- **THEN** the cost is returned based on game balance configuration

#### Scenario: Get cost for tier 2 in epoch 2
- **WHEN** GameStats.GetDefenseStanceCost is called with epoch 2 and tier 2
- **THEN** the cost is returned based on game balance configuration

### Requirement: Defense stance cost attribute in UnitEntity
The system SHALL include DefenseStanceCost attribute in UnitEntity (similar to UnlockCost).

#### Scenario: UnitEntity contains defense stance cost
- **WHEN** UnitEntity is created for a tier
- **THEN** DefenseStanceCost attribute is initialized with value from GameStats.GetDefenseStanceCost

#### Scenario: UnitModel exposes defense stance cost
- **WHEN** accessing UnitModel.DefenseStanceCost
- **THEN** the cost from UnitEntity is returned

### Requirement: Tier unlock requirement for defense stance purchase
The system SHALL require that units of a tier are unlocked before the defense stance ability for that tier can be purchased.

#### Scenario: Attempt purchase without tier unlock
- **WHEN** player attempts to purchase defense stance for a tier whose units are not unlocked
- **THEN** the purchase is blocked
- **AND** no crystals are deducted

#### Scenario: Purchase after tier unlock
- **WHEN** player has unlocked units for a tier
- **THEN** the defense stance ability for that tier can be purchased

### Requirement: Defense stance purchase persistence
The system SHALL persist the purchased state of tier defense stance abilities in EpochStorageData.

#### Scenario: Save purchased defense stance
- **WHEN** player purchases defense stance ability for a tier
- **THEN** the purchased state is saved in EpochStorageData
- **AND** the ability remains available after game restart

#### Scenario: Load purchased defense stance
- **WHEN** game is loaded
- **THEN** previously purchased tier defense stance abilities are loaded from EpochStorageData
