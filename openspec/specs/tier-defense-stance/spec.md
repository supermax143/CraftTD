## ADDED Requirements

### Requirement: Defense stance activation for tier units
The system SHALL allow activation of defense stance for all units of a specific tier during battle.

#### Scenario: Activate defense stance for tier 1 units
- **WHEN** player activates defense stance for tier 1 units
- **THEN** all tier 1 units on the battlefield enter defense stance
- **AND** units stop moving and play idle animation
- **AND** units remain in defense stance until deactivated or battle ends

#### Scenario: Activate defense stance for tier 2 units
- **WHEN** player activates defense stance for tier 2 units
- **THEN** all tier 2 units on the battlefield enter defense stance
- **AND** units stop moving and play idle animation

### Requirement: Defense stance deactivation
The system SHALL allow deactivation of defense stance for units of a specific tier during battle.

#### Scenario: Deactivate defense stance
- **WHEN** player deactivates defense stance for a tier
- **THEN** all units of that tier resume normal movement behavior
- **AND** units continue toward their target or path

### Requirement: Defense stance attack behavior
The system SHALL allow units in defense stance to attack enemies within attack range.

#### Scenario: Enemy enters attack range
- **WHEN** an enemy unit enters the attack range of a unit in defense stance
- **THEN** the unit in defense stance attacks the enemy
- **AND** the unit does not move from its position

#### Scenario: Enemy dies while attacking
- **WHEN** the enemy unit being attacked dies
- **THEN** the unit in defense stance returns to idle animation
- **AND** the unit remains in defense stance at its position

### Requirement: Defense stance availability check
The system SHALL only allow defense stance activation for tiers where the ability has been purchased.

#### Scenario: Attempt to activate without purchase
- **WHEN** player attempts to activate defense stance for a tier without purchasing the ability
- **THEN** the activation is blocked
- **AND** UI indicates the ability is not available

#### Scenario: Activate after purchase
- **WHEN** player has purchased defense stance ability for a tier
- **THEN** the defense stance activation is available for that tier
