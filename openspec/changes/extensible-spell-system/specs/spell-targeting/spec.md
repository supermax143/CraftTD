# spell-targeting Specification

## Purpose
Определяет механизмы выбора целей для Impact: Unit, Field, Area, Direction, Point, Cone, Line.

## ADDED Requirements

### Requirement: SpellTargetingType enum
The system SHALL define SpellTargetingType enum with targeting types.

#### Scenario: Enum contains NoTarget
- **WHEN** SpellTargetingType enum is defined
- **THEN** it includes NoTarget value

#### Scenario: Enum contains Unit
- **WHEN** SpellTargetingType enum is defined
- **THEN** it includes Unit value

#### Scenario: Enum contains Field
- **WHEN** SpellTargetingType enum is defined
- **THEN** it includes Field value

#### Scenario: Enum contains Area
- **WHEN** SpellTargetingType enum is defined
- **THEN** it includes Area value

#### Scenario: Enum contains Direction
- **WHEN** SpellTargetingType enum is defined
- **THEN** it includes Direction value

#### Scenario: Enum contains Self
- **WHEN** SpellTargetingType enum is defined
- **THEN** it includes Self value

#### Scenario: Enum contains Point
- **WHEN** SpellTargetingType enum is defined
- **THEN** it includes Point value

#### Scenario: Enum contains Cone
- **WHEN** SpellTargetingType enum is defined
- **THEN** it includes Cone value

#### Scenario: Enum contains Line
- **WHEN** SpellTargetingType enum is defined
- **THEN** it includes Line value

### Requirement: Unit targeting
The system SHALL support targeting specific units.

#### Scenario: Target single unit
- **WHEN** spell with Unit targeting is cast on a unit
- **THEN** the selected unit becomes the target

#### Scenario: Target multiple units
- **WHEN** spell with Unit targeting supports multiple targets
- **THEN** multiple units can be selected as targets

### Requirement: Field targeting
The system SHALL support targeting a field area.

#### Scenario: Target field with radius
- **WHEN** spell with Field targeting is cast
- **THEN** all units within radius are selected as targets

#### Scenario: Field targeting parameters
- **WHEN** spell has Field targeting
- **THEN** SpellDefinition contains radius parameter

### Requirement: Area targeting
The system SHALL support targeting a specific area.

#### Scenario: Target area
- **WHEN** spell with Area targeting is cast
- **THEN** all units in the area are selected as targets

### Requirement: Direction targeting
The system SHALL support targeting in a direction.

#### Scenario: Target in direction
- **WHEN** spell with Direction targeting is cast
- **THEN** targets are selected along the direction

#### Scenario: Direction targeting parameters
- **WHEN** spell has Direction targeting
- **THEN** SpellDefinition contains direction and range parameters

### Requirement: Self targeting
The system SHALL support targeting the caster.

#### Scenario: Target self
- **WHEN** spell with Self targeting is cast
- **THEN** the caster becomes the target

### Requirement: Point targeting
The system SHALL support targeting a specific point.

#### Scenario: Target point
- **WHEN** spell with Point targeting is cast at a location
- **THEN** the point becomes the target location

### Requirement: Cone targeting
The system SHALL support targeting a cone area.

#### Scenario: Target cone
- **WHEN** spell with Cone targeting is cast
- **THEN** units within the cone are selected as targets

#### Scenario: Cone targeting parameters
- **WHEN** spell has Cone targeting
- **THEN** SpellDefinition contains angle and range parameters

### Requirement: Line targeting
The system SHALL support targeting a line area.

#### Scenario: Target line
- **WHEN** spell with Line targeting is cast
- **THEN** units along the line are selected as targets

#### Scenario: Line targeting parameters
- **WHEN** spell has Line targeting
- **THEN** SpellDefinition contains length and width parameters
