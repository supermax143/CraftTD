# Shop Assets Setup Guide

This guide explains how to create and configure the Shop assets in Unity Editor.

## Prerequisites

The following ScriptableObject classes have been implemented and are ready to use:
- `ShopConfig` - Container for shop items
- `ShopItemConfig` - Individual shop item configuration

## Step 1: Create ShopConfig Asset

1. In Unity Editor, navigate to `Assets/Data/Shop/` folder (create if doesn't exist)
2. Right-click in the Project window
3. Select `Create > CraftTD > ShopConfig`
4. Name the asset `ShopConfig`

## Step 2: Create ShopItemConfig Assets

Create the following ShopItemConfig assets in `Assets/Data/Shop/`:

### Game Currency Purchases

#### 1. Money Pack (Small)
- **Id**: `money_pack_small`
- **Display Name**: `Money Pack (Small)`
- **Description**: `Get 100 Money instantly!`
- **Icon**: (assign appropriate sprite)
- **Payment Type**: `GameCurrency`
- **Currency Type**: `Crystal`
- **Price**: `50`
- **Is Consumable**: `true`
- **Localized Price Placeholder**: `50 💎`
- **Reward Type**: `Resource`
- **Reward Resource Type**: `Money`
- **Reward Amount**: `100`

#### 2. Crystal Pack (Small)
- **Id**: `crystal_pack_small`
- **Display Name**: `Crystal Pack (Small)`
- **Description**: `Get 50 Crystals instantly!`
- **Icon**: (assign appropriate sprite)
- **Payment Type**: `GameCurrency`
- **Currency Type**: `Money`
- **Price**: `500`
- **Is Consumable**: `true`
- **Localized Price Placeholder**: `500 💰`
- **Reward Type**: `Resource`
- **Reward Resource Type**: `Crystal`
- **Reward Amount**: `50`

### Real Money Purchases

#### 3. Starter Pack (Non-Consumable)
- **Id**: `starter_pack`
- **Display Name**: `Starter Pack`
- **Description**: `One-time purchase! Get 1000 Money and 100 Crystals`
- **Icon**: (assign appropriate sprite)
- **Payment Type**: `RealMoney`
- **Currency Type**: `Money` (unused for real money)
- **Price**: `0` (unused for real money)
- **Is Consumable**: `false`
- **Localized Price Placeholder**: `$0.99`
- **Reward Type**: `Resource`
- **Reward Resource Type**: `Money`
- **Reward Amount**: `1000`

#### 4. Crystal Pack (Consumable)
- **Id**: `real_crystal_pack`
- **Display Name**: `Crystal Pack (Premium)`
- **Description**: `Get 500 Crystals!`
- **Icon**: (assign appropriate sprite)
- **Payment Type**: `RealMoney`
- **Currency Type**: `Money` (unused for real money)
- **Price**: `0` (unused for real money)
- **Is Consumable**: `true`
- **Localized Price Placeholder**: `$4.99`
- **Reward Type**: `Resource`
- **Reward Resource Type**: `Crystal`
- **Reward Amount**: `500`

## Step 3: Configure ShopConfig

1. Select the `ShopConfig` asset created in Step 1
2. In the Inspector, find the `Items` list
3. Set `Size` to `4` (or more if you added additional items)
4. Drag and drop each ShopItemConfig asset into the list elements:
   - Element 0: `money_pack_small`
   - Element 1: `crystal_pack_small`
   - Element 2: `starter_pack`
   - Element 3: `real_crystal_pack`

## Step 4: Assign ShopConfig to UnityInstaller

1. Find the UnityInstaller component in the scene (likely on a GameManager or Bootstrap object)
2. In the Inspector, locate the `Shop Config` field
3. Drag and drop the `ShopConfig` asset from `Assets/Data/Shop/ShopConfig` into this field

## Step 5: Testing

### Test ShopWindow Opening
```csharp
// In any script with access to IWindowsController:
windowsController.ShowWindow<ShopWindow>();
```

### Test Scenarios

1. **Game Currency Purchase**:
   - Open ShopWindow
   - Click "Buy" on Money Pack (Small)
   - Verify: Crystal balance decreases by 50, Money increases by 100

2. **Real Money Purchase (Non-Consumable)**:
   - Use DummyPurchasesController (debug mode)
   - Purchase Starter Pack
   - Verify: Purchase completes, reward granted
   - Try purchasing again - should be blocked (already purchased)

3. **Real Money Purchase (Consumable)**:
   - Use DummyPurchasesController (debug mode)
   - Purchase Crystal Pack (Premium)
   - Verify: Crystal balance increases by 500
   - Purchase again - should succeed (consumable)

## File Structure

After setup, your Assets/Data/Shop/ folder should contain:
```
Assets/Data/Shop/
├── SHOP_SETUP_GUIDE.md
├── ShopConfig.asset
├── money_pack_small.asset
├── crystal_pack_small.asset
├── starter_pack.asset
└── real_crystal_pack.asset
```

## Notes

- Sprite icons should be assigned for better visual presentation
- Localization keys should be used for production (currently using placeholder strings)
- Real money product IDs should match your store configuration (Apple App Store, Google Play, etc.)