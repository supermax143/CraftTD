## ADDED Requirements

### Requirement: Defense stance idle behavior
The system SHALL make units in defense stance play idle animation while stationary.

#### Scenario: Enter defense stance
- **WHEN** a unit enters defense stance
- **THEN** the unit stops moving
- **AND** the unit plays idle animation
- **AND** the unit remains at its current position

### Requirement: Defense stance attack trigger
The system SHALL trigger attack when an enemy unit enters attack range while in defense stance.

#### Scenario: Enemy enters attack range
- **WHEN** an enemy unit enters the attack range of a unit in defense stance
- **THEN** the unit transitions to attack state
- **AND** the unit attacks the enemy
- **AND** the unit does not move from its position

### Requirement: Defense stance return to idle
The system SHALL return units to idle animation after attacking an enemy in defense stance.

#### Scenario: Enemy dies after attack
- **WHEN** the enemy being attacked dies
- **THEN** the unit returns to idle animation
- **AND** the unit remains in defense stance at its position
- **AND** the unit continues to monitor for enemies in attack range

### Requirement: Defense stance exit behavior
The system SHALL allow units to exit defense stance and resume normal movement.

#### Scenario: Deactivate defense stance
- **WHEN** defense stance is deactivated for a unit's tier
- **THEN** the unit exits defense stance state
- **AND** the unit resumes normal movement behavior
- **AND** the unit continues toward its target or path
