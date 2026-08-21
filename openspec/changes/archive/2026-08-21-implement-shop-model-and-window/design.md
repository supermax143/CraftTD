## Context

Проект Unity с DI через Zenject и слоистой архитектурой:
- Core.Application (отдельная asmdef) — бизнес-логика, модели, хранилища данных
- Unity.Infrastructure — контроллеры, сервисы (референсит Core.Application)
- Unity.Presentation — UI, окна, view-компоненты

Существующие компоненты:
- InventoryModel — управление ресурсами (Money, Crystal)
- IPurchasesController — интерфейс для покупок за реальные деньги (GPPurchasesController, DummyPurchasesController)
- PurchasesStorageData — хранение non-consumable покупок
- ScriptableObject конфиги (GameStats, ChronologyInfo)
- WindowBase + IWindowsController — система окон
- MainModel — корневая модель, создающая подмодели (InventoryModel, EpochModel)

Архитектурное ограничение: Core.Application не может референсить Unity.Infrastructure из-за asmdef-границ. Зависимость однонаправленная: Infrastructure → Core.Application.

## Goals / Non-Goals

**Goals:**
- Единая модель магазина для покупок за игровую и реальную валюту
- ScriptableObject конфигурация айтемов магазина
- UI окно магазина с динамическим списком айтемов
- Интеграция с существующей системой покупок (IPurchasesController)
- Соблюдение asmdef-границ (ShopModel в Core.Application без прямой зависимости от IPurchasesController)

**Non-Goals:**
- Полная реализация RewardType.Item (только enum для будущего расширения)
- Автоматическое обновление цен реальных покупок из стора (цены отображаются как "..." или из конфига)
- Локализация DisplayName/Description через существующий модуль (хардкод в MVP)
- Сложная система скидок/акций

## Decisions

### ShopModel в Core.Application без прямой зависимости от IPurchasesController

**Rationale:** Core.Application asmdef не видит Unity.Infrastructure. ShopModel не может инжектить IPurchasesController напрямую.

**Alternative:** Вынести ShopModel в Infrastructure или создать общий слой.
**Rejected:** Нарушает слоистую архитектуру, бизнес-логика должна быть в Core.Application.

**Solution:** ShopModel предоставляет метод GrantRealMoneyPurchase(string itemId), который вызывается извне (из GPPurchasesController/DummyPurchasesController) после успешного колбэка OnPurchaseComplete. Это инверсия контроля: Infrastructure вызывает Core.Application модель.

### ShopModel создаётся внутри MainModel.Init(), не отдельным Zenject-биндингом

**Rationale:** InventoryModel создаётся вручную внутри MainModel.Init(), а не биндится как синглтон. Для консистентности повторяем этот паттерн.

**Alternative:** Биндить ShopModel как To<ShopModel>().AsSingle() в UnityInstaller.
**Rejected:** Отличается от существующего паттерна для подмоделей (InventoryModel, EpochModel).

**Solution:** Добавить IShopModel Shop { get; } в IMainModel/MainModel, создавать в MainModel.Init() вместе с InventoryModel.

### Единый ShopConfig для всех типов айтемов

**Rationale:** Гейм-дизайн может смешивать айтемы за игровую и реальную валюту в одном магазине. Единый конфиг упрощает управление.

**Alternative:** Раздельные GameCurrencyShopConfig и RealMoneyShopConfig.
**Rejected:** Усложняет архитектуру без явной выгоды, enum PaymentType достаточно для различия.

**Solution:** ShopItemConfig содержит enum PaymentType и поля для обоих типов (условные поля через сериализацию).

### Динамическое создание ShopItemView через InstantiatePrefabForComponent

**Rationale:** Количество айтемов задаётся конфигом, не фиксировано сценой. Динамическое создание гибче.

**Alternative:** Фиксированный список [SerializeField] ShopItemView в сцене (как UpgradeWindow).
**Rejected:** Требует синхронизации сцены и конфига, менее гибко для гейм-дизайна.

**Solution:** ShopWindow содержит Transform _itemsContainer и префаб ShopItemView, создаёт вьюхи в цикле по конфигу.

### RewardType.Item только как enum, без реализации

**Rationale:** В текущей задаче нужны только ресурсы (Money, Crystal). Предметы могут быть добавлены позже.

**Alternative:** Полностью реализовать систему предметов.
**Rejected:** Увеличивает объём работы без требования в задаче.

**Solution:** Добавить enum RewardType с Resource и Item, реализовать только Resource ветку.

## Risks / Trade-offs

**Risk:** ShopModel.GrantRealMoneyPurchase может быть вызван дважды для одного non-consumable айтема (баг в IPurchasesController).
**Mitigation:** ShopModel проверяет IsPurchased перед выдачей награды и записью в PurchasesStorageData. Двойной вызов безопасен.

**Risk:** Цены реальных покупок не обновляются из стора, отображаются как "...".
**Mitigation:** В будущем можно добавить кэширование цен в ShopModel через метод SetRealMoneyPrice(string id, string price), вызываемый из GPPurchasesController.OnProductsFetched.

**Trade-off:** Локализация DisplayName/Description хардкодится в ScriptableObject вместо использования существующего модуля локализации.
**Reason:** Упрощение MVP, можно заменить позже без изменения архитектуры.

**Risk:** Consumable покупки за реальные деньги могут быть заблокированы PurchasesStorageData.
**Mitigation:** PurchasesStorageData.AddPurchase вызывается только для non-consumable (проверка IsConsumable в ShopItemConfig).

## Migration Plan

1. Создать ScriptableObject классы (ShopItemConfig, ShopConfig)
2. Создать ShopModel в Core.Application
3. Добавить ShopModel в MainModel (IShopModel свойство, создание в Init())
4. Доработать GPPurchasesController и DummyPurchasesController (инжект ShopModel, вызов GrantRealMoneyPurchase)
5. Создать ShopWindow и ShopItemView в Unity/Presentation
6. Добавить биндинг ShopConfig в UnityInstaller
7. Создать ассет ShopConfig в проекте и настроить тестовые айтемы

**Rollback:** Удалить файлы, убрать биндинг из UnityInstaller, удалить ShopModel из MainModel. Обратная совместимость не нарушается (новый функционал, не изменение существующего).

## Open Questions

1. **Где хранить префаб ShopItemView?** В Resources/ или Addressables? Рекомендуется Addressables для консистентности с другими окнами.
2. **Локализация цен реальных покупок:** Нужно ли кэшировать цены из стора в ShopModel или достаточно отображать "..."? Решено: MVP с "...", расширение позже.
3. **RewardType.Item:** Какие типы предметов будут? Оставлено на будущее, только enum.
