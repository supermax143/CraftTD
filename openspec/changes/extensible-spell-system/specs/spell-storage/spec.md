# spell-storage Specification

## Purpose
Определяет сохранение и загрузку прогресса спеллов через существующую DataStorage архитектуру.

## ADDED Requirements

### Requirement: SpellProgressStorageData
The system SHALL define SpellProgressStorageData containing spell progress for all spells.

#### Scenario: SpellProgressStorageData contains spell progress
- **WHEN** SpellProgressStorageData is defined
- **THEN** it contains array of spell progress entries

#### Scenario: Spell progress entry contains Id
- **WHEN** spell progress entry is created
- **THEN** it contains SpellId field

#### Scenario: Spell progress entry contains IsUnlocked
- **WHEN** spell progress entry is created
- **THEN** it contains IsUnlocked field

#### Scenario: Spell progress entry contains Level
- **WHEN** spell progress entry is created
- **THEN** it contains Level field

### Requirement: SpellProgressStorage
The system SHALL define SpellProgressStorage using existing IStorageProvider.

#### Scenario: SpellProgressStorage uses IStorageProvider
- **WHEN** SpellProgressStorage is created
- **THEN** it uses IStorageProvider for persistence

#### Scenario: SpellProgressStorage uses StringStorageVariable
- **WHEN** SpellProgressStorage is created
- **THEN** it uses StringStorageVariable for storage key

#### Scenario: SpellProgressStorage uses Newtonsoft.Json
- **WHEN** SpellProgressStorage serializes data
- **THEN** it uses Newtonsoft.Json for serialization

### Requirement: Save spell progress
The system SHALL save spell progress to storage.

#### Scenario: Save unlocked spell
- **WHEN** spell is unlocked and progress is saved
- **THEN** storage contains entry with IsUnlocked=true

#### Scenario: Save spell level
- **WHEN** spell is upgraded to level 3 and progress is saved
- **THEN** storage contains entry with Level=3

#### Scenario: Save multiple spells
- **WHEN** multiple spells have progress and are saved
- **THEN** storage contains entries for all spells

### Requirement: Load spell progress
The system SHALL load spell progress from storage.

#### Scenario: Load unlocked spell
- **WHEN** storage contains entry with IsUnlocked=true and progress is loaded
- **THEN** SpellModel has IsUnlocked=true

#### Scenario: Load spell level
- **WHEN** storage contains entry with Level=3 and progress is loaded
- **THEN** SpellModel has Level=3

#### Scenario: Load locked spell
- **WHEN** storage contains entry with IsUnlocked=false and progress is loaded
- **THEN** SpellModel has IsUnlocked=false

#### Scenario: Load multiple spells
- **WHEN** storage contains entries for multiple spells and progress is loaded
- **THEN** all SpellModels are restored with correct progress

### Requirement: Storage JSON format
The system SHALL use JSON format for spell progress storage.

#### Scenario: JSON structure
- **WHEN** spell progress is serialized to JSON
- **THEN** it has structure with Spells array containing Id, IsUnlocked, Level

#### Scenario: JSON example
- **WHEN** spell progress is serialized
- **THEN** JSON matches format: {"Spells": [{"Id": "fireball", "IsUnlocked": true, "Level": 3}]}

### Requirement: Progress restoration after restart
The system SHALL restore spell progress after game restart.

#### Scenario: Restore after restart
- **WHEN** game is restarted and spell progress is loaded
- **THEN** SpellModels have same IsUnlocked and Level as before restart

#### Scenario: Restore multiple spells after restart
- **WHEN** game is restarted and multiple spell progress entries are loaded
- **THEN** all SpellModels are restored with correct progress

### Requirement: Minimal storage data
The system SHALL store only minimal necessary progress data.

#### Scenario: Store only progress fields
- **WHEN** spell progress is saved
- **THEN** only SpellId, IsUnlocked, and Level are stored

#### Scenario: Don't store SpellDefinition
- **WHEN** spell progress is saved
- **THEN** SpellDefinition is not stored in progress data

#### Scenario: Don't store calculated stats
- **WHEN** spell progress is saved
- **THEN** calculated runtime stats are not stored in progress data
