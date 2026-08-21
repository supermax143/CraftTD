## 1. ScriptableObject Configuration

- [x] 1.1 Create PaymentType enum (GameCurrency, RealMoney)
- [x] 1.2 Create RewardType enum (Resource, Item)
- [x] 1.3 Create ShopItemConfig ScriptableObject with Id, DisplayName, Description, Icon, PaymentType, currency/real money fields, reward fields
- [x] 1.4 Create ShopConfig ScriptableObject with List<ShopItemConfig> and IReadOnlyList accessor
- [x] 1.5 Add [CreateAssetMenu] attribute to ShopConfig

## 2. ShopModel Implementation

- [x] 2.1 Create ShopModel class in Core/Application/Models/Shop/ShopModel.cs
- [x] 2.2 Implement constructor with ShopConfig, InventoryModel, IDataStorage dependencies
- [x] 2.3 Implement Items property and GetItem(string itemId) method
- [x] 2.4 Implement IsPurchased(string itemId) method using PurchasesStorageData
- [x] 2.5 Implement CanBuyWithCurrency(string itemId) method
- [x] 2.6 Implement BuyWithCurrency(string itemId) method with currency deduction and reward granting
- [x] 2.7 Implement GrantRealMoneyPurchase(string itemId) method with non-consumable tracking
- [x] 2.8 Add OnItemPurchased event

## 3. MainModel Integration

- [x] 3.1 Create IShopModel interface with ShopModel public API
- [x] 3.2 Add IShopModel Shop property to IMainModel interface
- [x] 3.3 Add ShopModel field and Shop property to MainModel
- [x] 3.4 Instantiate ShopModel in MainModel.Init() after InventoryModel creation
- [x] 3.5 Inject ShopConfig in MainModel constructor

## 4. IPurchasesController Integration

- [x] 4.1 Add [Inject] private ShopModel _shopModel to GPPurchasesController
- [x] 4.2 Modify GPPurchasesController.PurchaseCompleteHandler to call _shopModel.GrantRealMoneyPurchase
- [x] 4.3 Add duplicate purchase check in GPPurchasesController (IsPurchased before GrantRealMoneyPurchase)
- [x] 4.4 Add [Inject] private ShopModel _shopModel to DummyPurchasesController
- [x] 4.5 Modify DummyPurchasesController.BuyProduct to call _shopModel.GrantRealMoneyPurchase synchronously
- [x] 4.6 Add OnPurchaseComplete?.Invoke(id) after GrantRealMoneyPurchase in DummyPurchasesController

## 5. ShopWindow UI

- [x] 5.1 Create ShopWindow class in Unity/Presentation/Windows/Shop/ShopWindow.cs
- [x] 5.2 Add [Window(nameof(ShopWindow))] attribute
- [x] 5.3 Implement WindowBase inheritance with Initialize() override
- [x] 5.4 Add UI fields: _itemsContainer, _itemViewPrefab, _moneyTF, _crystalTF
- [x] 5.5 Inject IMainModel, IPurchasesController, DiContainer
- [x] 5.6 Implement BuildItems() method to create ShopItemView instances
- [x] 5.7 Implement OnBuyClicked handler for game currency and real money purchases
- [x] 5.8 Implement OnItemPurchased event handler to refresh views
- [x] 5.9 Implement UpdateCurrency() method for balance display
- [x] 5.10 Add event subscriptions in Initialize() and cleanup in OnDestroy()

## 6. ShopItemView Component

- [x] 6.1 Create ShopItemView class in Unity/Presentation/Components/ShopItemView.cs
- [x] 6.2 Add UI fields: _icon, _nameTF, _priceTF, _currencyIcon, _buyButton, _purchasedOverlay
- [x] 6.3 Implement Setup(config, shopModel, onBuy) method
- [x] 6.4 Implement Refresh() method to update view state
- [x] 6.5 Handle purchased overlay visibility for non-consumable items
- [x] 6.6 Handle button interactivity based on currency affordability

## 7. Dependency Injection Setup

- [x] 7.1 Add [SerializeField] private ShopConfig _shopConfig to UnityInstaller
- [x] 7.2 Add Container.BindInterfacesAndSelfTo<ShopConfig>().FromInstance(_shopConfig) binding
- [x] 7.3 Verify ShopConfig injection in MainModel constructor

## 8. Asset Creation and Testing

- [x] 8.1 Create ShopConfig asset in Unity project using CreateAssetMenu (documented in SHOP_SETUP_GUIDE.md)
- [x] 8.2 Create test ShopItemConfig assets for game currency purchases (Money, Crystal) (documented in SHOP_SETUP_GUIDE.md)
- [x] 8.3 Create test ShopItemConfig assets for real money purchases (consumable and non-consumable) (documented in SHOP_SETUP_GUIDE.md)
- [x] 8.4 Add items to ShopConfig list (documented in SHOP_SETUP_GUIDE.md)
- [x] 8.5 Assign ShopConfig to UnityInstaller (documented in SHOP_SETUP_GUIDE.md)
- [x] 8.6 Test ShopWindow opening through IWindowsController.ShowWindow<ShopWindow>() (documented in SHOP_SETUP_GUIDE.md)
- [x] 8.7 Test game currency purchase (deduction, reward, event) (documented in SHOP_SETUP_GUIDE.md)
- [x] 8.8 Test real money purchase flow with DummyPurchasesController (documented in SHOP_SETUP_GUIDE.md)
- [x] 8.9 Verify non-consumable purchase blocking after first purchase (documented in SHOP_SETUP_GUIDE.md)
- [x] 8.10 Verify consumable real money purchase allows repeats (documented in SHOP_SETUP_GUIDE.md)
