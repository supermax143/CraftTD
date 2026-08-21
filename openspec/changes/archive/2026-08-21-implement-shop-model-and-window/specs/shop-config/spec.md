## ADDED Requirements

### Requirement: Shop item config defines unique identifier
The system SHALL require each shop item to have a unique identifier string.

#### Scenario: Item ID is unique
- **WHEN** ShopItemConfig is created
- **THEN** Id field must be unique across all items in ShopConfig

### Requirement: Shop item config defines display name
The system SHALL require each shop item to have a display name for UI.

#### Scenario: Display name is provided
- **WHEN** ShopItemConfig is created
- **THEN** DisplayName field contains the text to display in UI

### Requirement: Shop item config defines icon
The system SHALL require each shop item to have an icon sprite.

#### Scenario: Icon is provided
- **WHEN** ShopItemConfig is created
- **THEN** Icon field contains a Sprite reference

### Requirement: Shop item config defines payment type
The system SHALL specify whether item is purchased with game currency or real money.

#### Scenario: Game currency payment type
- **WHEN** PaymentType is set to GameCurrency
- **THEN** CurrencyType and Price fields are used for purchase

#### Scenario: Real money payment type
- **WHEN** PaymentType is set to RealMoney
- **THEN** IsConsumable field determines if purchase can be repeated

### Requirement: Shop item config defines game currency price
The system SHALL specify currency type and amount for game currency purchases.

#### Scenario: Money currency price
- **WHEN** PaymentType is GameCurrency and CurrencyType is Money
- **THEN** Price field specifies the Money amount required

#### Scenario: Crystal currency price
- **WHEN** PaymentType is GameCurrency and CurrencyType is Crystal
- **THEN** Price field specifies the Crystal amount required

### Requirement: Shop item config defines consumability for real money purchases
The system SHALL specify whether real money purchase can be repeated.

#### Scenario: Non-consumable real money purchase
- **WHEN** PaymentType is RealMoney and IsConsumable is false
- **THEN** item can only be purchased once and is tracked in PurchasesStorageData

#### Scenario: Consumable real money purchase
- **WHEN** PaymentType is RealMoney and IsConsumable is true
- **THEN** item can be purchased multiple times without tracking

### Requirement: Shop item config defines reward
The system SHALL specify what player receives after purchase.

#### Scenario: Resource reward
- **WHEN** RewardType is Resource
- **THEN** RewardResourceType and RewardAmount fields specify the resource and quantity granted

#### Scenario: Item reward
- **WHEN** RewardType is Item
- **THEN** item-specific reward fields are used (future implementation)

### Requirement: Shop config contains list of shop items
The system SHALL provide a container for all shop item configurations.

#### Scenario: Shop config holds items
- **WHEN** ShopConfig is created
- **THEN** Items field contains List<ShopItemConfig> with all shop items

#### Scenario: Shop config exposes read-only items
- **WHEN** client accesses ShopConfig.Items
- **THEN** system returns IReadOnlyList<ShopItemConfig>
