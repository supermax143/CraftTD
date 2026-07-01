## Why

В игре отсутствует система магазина для покупки айтемов за игровую валюту (Money, Crystal) и за реальные деньги. Это ограничивает монетизацию и progression-систему. Реализация магазина необходима для:
- Покупки ресурсов за игровую валюту (мгновенно, без сети)
- Покупки премиум-айтемов за реальные деньги через GamePush SDK
- Единый конфигурационный подход через ScriptableObject

## What Changes

- **ShopModel**: новая модель в Core.Application для управления состоянием магазина, покупками за игровую валюту и выдачей наград
- **ShopWindow**: новое UI окно в Unity/Presentation для отображения списка айтемов магазина
- **ShopItemConfig/ShopConfig**: ScriptableObject конфигурации для определения айтемов магазина (цена, тип оплаты, награда)
- **Интеграция с IPurchasesController**: доработка GPPurchasesController и DummyPurchasesController для вызова ShopModel.GrantRealMoneyPurchase после успешной оплаты
- **IShopModel интерфейс**: добавление в MainModel или отдельный Zenject-биндинг для доступа к ShopModel

## Capabilities

### New Capabilities
- `shop-model`: модель магазина с поддержкой покупок за игровую валюту (Money/Crystal) и реальные деньги, управление состоянием "куплено" для non-consumable айтемов
- `shop-window`: UI окно магазина с динамическим списком айтемов, отображением баланса валюты и кнопками покупки
- `shop-config`: ScriptableObject конфигурация айтемов магазина с поддержкой разных типов оплаты и наград

### Modified Capabilities
- `purchases-controller`: интеграция с ShopModel для выдачи наград после успешной реальной покупки (без изменения требований, только implementation)

## Impact

- **Core.Application**: добавлены ShopModel, ShopItemConfig, ShopConfig, enum PaymentType/RewardType
- **Unity.Infrastructure**: GPPurchasesController и DummyPurchasesController инжектят ShopModel и вызывают GrantRealMoneyPurchase
- **Unity.Presentation**: добавлены ShopWindow и ShopItemView префаб
- **Unity.Installers**: добавлен биндинг ShopConfig в UnityInstaller
- **MainModel**: добавлено свойство Shop для доступа к ShopModel
- **Асmdef-ограничения**: ShopModel не зависит от IPurchasesController (соблюдается однонаправленная зависимость Infrastructure → Core.Application)
