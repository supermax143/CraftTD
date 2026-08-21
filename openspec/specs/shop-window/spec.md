## ADDED Requirements

### Requirement: Shop window displays list of shop items
The system SHALL display all shop items configured in ShopModel.

#### Scenario: Window opens with all items
- **WHEN** ShopWindow is initialized
- **THEN** system creates ShopItemView for each item in ShopModel.Items and adds them to items container

### Requirement: Shop window displays current currency balances
The system SHALL display player's current Money and Crystal balances.

#### Scenario: Display currency balances on open
- **WHEN** ShopWindow is initialized
- **THEN** system updates money and crystal text fields with values from InventoryModel

#### Scenario: Update currency balances on change
- **WHEN** InventoryModel.OnMoneyChanged or OnCrystalChanged event is invoked
- **THEN** system updates corresponding text field with new value

### Requirement: Shop window handles game currency purchase clicks
The system SHALL process purchase through ShopModel when player clicks buy button for game currency item.

#### Scenario: Click buy button for game currency item
- **WHEN** player clicks buy button on ShopItemView with PaymentType.GameCurrency
- **THEN** system calls ShopModel.BuyWithCurrency(itemId)

### Requirement: Shop window handles real money purchase clicks
The system SHALL initiate purchase through IPurchasesController when player clicks buy button for real money item.

#### Scenario: Click buy button for real money item
- **WHEN** player clicks buy button on ShopItemView with PaymentType.RealMoney
- **THEN** system calls IPurchasesController.BuyProduct(itemId)

### Requirement: Shop window refreshes item views after purchase
The system SHALL update item view state when an item is purchased.

#### Scenario: Refresh item after purchase
- **WHEN** ShopModel.OnItemPurchased event is invoked
- **THEN** system calls Refresh() on the corresponding ShopItemView

### Requirement: Shop window cleans up event subscriptions on destroy
The system SHALL unsubscribe from all events when window is destroyed.

#### Scenario: Unsubscribe on destroy
- **WHEN** ShopWindow is destroyed
- **THEN** system unsubscribes from InventoryModel.OnMoneyChanged, OnCrystalChanged, and ShopModel.OnItemPurchased events
