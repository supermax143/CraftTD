# spell-activation Specification

## Purpose
Определяет механизмы когда и как спелл начинает выполнение: Instant, CastTime, Channel, Trigger, Toggle.

## ADDED Requirements

### Requirement: SpellActivationType enum
The system SHALL define SpellActivationType enum with activation types.

#### Scenario: Enum contains Instant
- **WHEN** SpellActivationType enum is defined
- **THEN** it includes Instant value

#### Scenario: Enum contains CastTime
- **WHEN** SpellActivationType enum is defined
- **THEN** it includes CastTime value

#### Scenario: Enum contains Channel
- **WHEN** SpellActivationType enum is defined
- **THEN** it includes Channel value

#### Scenario: Enum contains Trigger
- **WHEN** SpellActivationType enum is defined
- **THEN** it includes Trigger value

#### Scenario: Enum contains Toggle
- **WHEN** SpellActivationType enum is defined
- **THEN** it includes Toggle value

### Requirement: Instant activation
The system SHALL support Instant activation where spell executes immediately.

#### Scenario: Instant spell executes immediately
- **WHEN** spell with Instant activation is cast
- **THEN** spell executes without delay

### Requirement: CastTime activation
The system SHALL support CastTime activation where spell has cast time before execution.

#### Scenario: CastTime spell has delay
- **WHEN** spell with CastTime activation is cast
- **THEN** spell executes after castTime duration

#### Scenario: CastTime can be interrupted
- **WHEN** spell with CastTime activation is being cast and caster is interrupted
- **THEN** spell execution is cancelled

### Requirement: Channel activation
The system SHALL support Channel activation where spell requires continuous channeling.

#### Scenario: Channel spell requires continuous input
- **WHEN** spell with Channel activation is cast
- **THEN** spell effect continues while channeling

#### Scenario: Channel can be interrupted
- **WHEN** spell with Channel activation is being channeled and caster is interrupted
- **THEN** spell effect stops immediately

### Requirement: Trigger activation
The system SHALL support Trigger activation where spell executes on specific condition.

#### Scenario: Trigger spell on condition
- **WHEN** trigger condition is met
- **THEN** spell executes automatically

### Requirement: Toggle activation
The system SHALL support Toggle activation where spell can be turned on and off.

#### Scenario: Toggle spell on
- **WHEN** spell with Toggle activation is activated
- **THEN** spell effect remains active until deactivated

#### Scenario: Toggle spell off
- **WHEN** spell with Toggle activation is deactivated
- **THEN** spell effect stops

### Requirement: Activation configuration parameters
The system SHALL allow activation-specific parameters in SpellDefinition.

#### Scenario: CastTime parameter
- **WHEN** spell has CastTime activation
- **THEN** SpellDefinition contains castTime parameter

#### Scenario: ChannelDuration parameter
- **WHEN** spell has Channel activation
- **THEN** SpellDefinition contains channelDuration parameter
