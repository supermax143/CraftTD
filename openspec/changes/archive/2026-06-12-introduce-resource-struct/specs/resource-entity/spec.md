## ADDED Requirements

### Requirement: Resource struct definition
The system SHALL define a Resource struct that encapsulates a resource type and its value.

#### Scenario: Create Money resource
- **WHEN** a Resource is created with type Money and value 100
- **THEN** the Resource struct contains type Money and value 100

#### Scenario: Create Food resource
- **WHEN** a Resource is created with type Food and value 50
- **THEN** the Resource struct contains type Food and value 50

### Requirement: ResourceType enum
The system SHALL define a ResourceType enum with at least Food and Money values.

#### Scenario: Enum contains Food
- **WHEN** ResourceType enum is defined
- **THEN** it includes Food value

#### Scenario: Enum contains Money
- **WHEN** ResourceType enum is defined
- **THEN** it includes Money value

#### Scenario: Enum is extensible
- **WHEN** a new resource type needs to be added
- **THEN** a new value can be added to ResourceType enum without breaking existing code

### Requirement: Resource value operations
The system SHALL support arithmetic operations on Resource values.

#### Scenario: Add resources of same type
- **WHEN** two Resources of type Money with values 10 and 20 are added
- **THEN** the result is a Resource of type Money with value 30

#### Scenario: Subtract resources of same type
- **WHEN** a Resource of type Food with value 50 is subtracted by a Resource of type Food with value 20
- **THEN** the result is a Resource of type Food with value 30

#### Scenario: Cannot add different resource types
- **WHEN** a Resource of type Money is added to a Resource of type Food
- **THEN** the operation throws an exception or returns an error

### Requirement: Resource comparison
The system SHALL support comparison operations on Resources of the same type.

#### Scenario: Compare equal resources
- **WHEN** two Resources of type Money with value 100 are compared
- **THEN** they are considered equal

#### Scenario: Compare greater than
- **WHEN** a Resource of type Food with value 50 is compared to a Resource of type Food with value 30
- **THEN** the first is considered greater than the second

#### Scenario: Cannot compare different resource types
- **WHEN** a Resource of type Money is compared to a Resource of type Food
- **THEN** the operation throws an exception or returns an error
