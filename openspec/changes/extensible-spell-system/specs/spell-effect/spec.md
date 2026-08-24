# spell-effect Specification

## Purpose
Определяет базовую систему эффектов с lifecycle (Apply, Update, Expire, Remove). Поддерживает instant, duration и periodic execution.

## ADDED Requirements

### Requirement: SpellEffect base class
The system SHALL define abstract SpellEffect base class with lifecycle methods.

#### Scenario: SpellEffect has Apply method
- **WHEN** SpellEffect is defined
- **THEN** it has Apply() method

#### Scenario: SpellEffect has Update method
- **WHEN** SpellEffect is defined
- **THEN** it has Update() method

#### Scenario: SpellEffect has Expire method
- **WHEN** SpellEffect is defined
- **THEN** it has Expire() method

#### Scenario: SpellEffect has Remove method
- **WHEN** SpellEffect is defined
- **THEN** it has Remove() method

### Requirement: Instant effect execution
The system SHALL support instant effects that complete immediately on Apply.

#### Scenario: DamageEffect as instant
- **WHEN** DamageEffect with Duration=0 is applied
- **THEN** damage is applied immediately and effect completes

#### Scenario: Instant effect skips Update
- **WHEN** instant effect is applied
- **THEN** Update() is not called

#### Scenario: Instant effect skips Expire
- **WHEN** instant effect is applied
- **THEN** Expire() is not called

### Requirement: Duration effect execution
The system SHALL support duration effects that create state for specified time.

#### Scenario: FreezeEffect with duration
- **WHEN** FreezeEffect with Duration=3 is applied
- **THEN** effect creates freeze state for 3 seconds

#### Scenario: Duration effect lifecycle
- **WHEN** duration effect is applied
- **THEN** it goes through Apply → Active → Expire → Remove

#### Scenario: Duration effect calls Update
- **WHEN** duration effect is active
- **THEN** Update() is called each frame

#### Scenario: Duration effect expires
- **WHEN** duration effect's duration elapses
- **THEN** Expire() is called and state is removed

### Requirement: Periodic effect execution
The system SHALL support periodic effects with tick interval.

#### Scenario: BurnEffect with tick interval
- **WHEN** BurnEffect with Duration=5 and TickInterval=1 is applied
- **THEN** damage is applied every 1 second for 5 seconds

#### Scenario: Periodic effect ticks
- **WHEN** periodic effect is active
- **THEN** effect logic is executed at each tick interval

#### Scenario: Periodic effect total ticks
- **WHEN** BurnEffect with Duration=5 and TickInterval=1 is applied
- **THEN** 5 ticks occur (at 1s, 2s, 3s, 4s, 5s)

### Requirement: DamageEffect
The system SHALL define DamageEffect that applies damage to target.

#### Scenario: DamageEffect applies damage
- **WHEN** DamageEffect with damage=100 is applied to target
- **THEN** target receives 100 damage

#### Scenario: DamageEffect as instant
- **WHEN** DamageEffect is applied
- **THEN** damage is applied immediately and effect completes

### Requirement: HealEffect
The system SHALL define HealEffect that heals target.

#### Scenario: HealEffect applies heal
- **WHEN** HealEffect with healAmount=50 is applied to target
- **THEN** target receives 50 heal

#### Scenario: HealEffect as instant
- **WHEN** HealEffect is applied
- **THEN** heal is applied immediately and effect completes

### Requirement: FreezeEffect
The system SHALL define FreezeEffect that prevents movement.

#### Scenario: FreezeEffect prevents movement
- **WHEN** FreezeEffect with Duration=3 is applied to target
- **THEN** target cannot move for 3 seconds

#### Scenario: FreezeEffect as duration
- **WHEN** FreezeEffect is applied
- **THEN** freeze state persists for duration then is removed

#### Scenario: FreezeEffect with movement speed modifier
- **WHEN** FreezeEffect with movementSpeedMultiplier=0 is applied
- **THEN** target movement speed is set to 0

### Requirement: Effect extensibility
The system SHALL allow adding new effects without changing existing spell code.

#### Scenario: Add SlowEffect
- **WHEN** SlowEffect is created
- **THEN** it can be used in existing spells without modifying spell code

#### Scenario: Add BurnEffect
- **WHEN** BurnEffect is created
- **THEN** it can be used in existing spells without modifying spell code

#### Scenario: Add PoisonEffect
- **WHEN** PoisonEffect is created
- **THEN** it can be used in existing spells without modifying spell code

### Requirement: Effect parameters
The system SHALL allow effects to have configurable parameters.

#### Scenario: DamageEffect damage parameter
- **WHEN** DamageEffect is created
- **THEN** it has configurable damage parameter

#### Scenario: FreezeEffect duration parameter
- **WHEN** FreezeEffect is created
- **THEN** it has configurable duration parameter

#### Scenario: BurnEffect tickInterval parameter
- **WHEN** BurnEffect is created
- **THEN** it has configurable tickInterval parameter
