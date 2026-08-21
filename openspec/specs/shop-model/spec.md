## ADDED Requirements

### Requirement: Shop model provides list of shop items
The system SHALL provide access to the complete list of shop items configured in ShopConfig.

#### Scenario: Get all shop items
- **WHEN** client accesses ShopModel.Items property
- **THEN** system returns IReadOnlyList<ShopItemConfig> containing all configured items

### Requirement: Shop model retrieves individual shop item by ID
The system SHALL allow retrieval of a specific shop item by its unique identifier.

#### Scenario: Get existing shop item
- **WHEN** client calls ShopModel.GetItem(itemId) with valid ID
- **THEN** system returns the corresponding ShopItemConfig

#### Scenario: Get non-existent shop item
- **WHEN** client calls ShopModel.GetItem(itemId) with invalid ID
- **THEN** system returns null

### Requirement: Shop model checks purchase status for non-consumable items
The system SHALL track which non-consumable real-money items have been purchased.

#### Scenario: Check purchased non-consumable item
- **WHEN** client calls ShopModel.IsPurchased(itemId) for a non-consumable item that was purchased
- **THEN** system returns true

#### Scenario: Check unpurchased non-consumable item
- **WHEN** client calls ShopModel.IsPurchased(itemId) for a non-consumable item that was not purchased
- **THEN** system returns false

#### Scenario: Check consumable item purchase status
- **WHEN** client calls ShopModel.IsPurchased(itemId) for a consumable item
- **THEN** system returns false regardless of purchase count

### Requirement: Shop model validates currency purchase affordability
The system SHALL check if player has sufficient game currency to purchase an item.

#### Scenario: Check affordable game currency purchase
- **WHEN** client calls ShopModel.CanBuyWithCurrency(itemId) for a game currency item and player has sufficient currency
- **THEN** system returns true

#### Scenario: Check unaffordable game currency purchase
- **WHEN** client calls ShopModel.CanBuyWithCurrency(itemId) for a game currency item and player lacks sufficient currency
- **THEN** system returns false

#### Scenario: Check real money item affordability
- **WHEN** client calls ShopModel.CanBuyWithCurrency(itemId) for a real money item
- **THEN** system returns false

### Requirement: Shop model processes game currency purchases
The system SHALL deduct game currency and grant reward when purchasing with game currency.

#### Scenario: Successful game currency purchase
- **WHEN** client calls ShopModel.BuyWithCurrency(itemId) with sufficient currency
- **THEN** system deducts price from InventoryModel, grants reward, invokes OnItemPurchased event, and returns true

#### Scenario: Failed game currency purchase due to insufficient funds
- **WHEN** client calls ShopModel.BuyWithCurrency(itemId) with insufficient currency
- **THEN** system does not deduct currency, does not grant reward, does not invoke event, and returns false

#### Scenario: Failed game currency purchase for real money item
- **WHEN** client calls ShopModel.BuyWithCurrency(itemId) for a real money item
- **THEN** system returns false without any state changes

### Requirement: Shop model grants rewards for real money purchases
The system SHALL grant rewards and track non-consumable purchases when called after successful real money payment.

#### Scenario: Grant non-consumable real money purchase
- **WHEN** client calls ShopModel.GrantRealMoneyPurchase(itemId) for a non-consumable item that was not previously purchased
- **THEN** system adds item to PurchasesStorageData, grants reward through InventoryModel, invokes OnItemPurchased event

#### Scenario: Grant consumable real money purchase
- **WHEN** client calls ShopModel.GrantRealMoneyPurchase(itemId) for a consumable item
- **THEN** system grants reward through InventoryModel, invokes OnItemPurchased event, does not add to PurchasesStorageData

#### Scenario: Grant duplicate non-consumable real money purchase
- **WHEN** client calls ShopModel.GrantRealMoneyPurchase(itemId) for a non-consumable item that was already purchased
- **THEN** system does not grant reward, does not invoke event, returns without changes

### Requirement: Shop model notifies on item purchase
The system SHALL notify subscribers when any item is successfully purchased.

#### Scenario: Event invoked on game currency purchase
- **WHEN** ShopModel successfully processes a game currency purchase
- **THEN** system invokes OnItemPurchased event with the item ID

#### Scenario: Event invoked on real money purchase grant
- **WHEN** ShopModel successfully grants a real money purchase
- **THEN** system invokes OnItemPurchased event with the item ID
