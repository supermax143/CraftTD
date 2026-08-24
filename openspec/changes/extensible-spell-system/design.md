## Context

Проект Unity CraftTD имеет существующую архитектуру с четким разделением на Application layer (бизнес-логика) и Unity layer (presentation). Существуют системы:
- DataStorage через IStorageProvider и StringStorageVariable
- AttributeModifier для модификации параметров (Add, Multiply, Override)
- UnitModel/EpochModel для управления прогрессом
- GameStats для конфигурационных данных через ScriptableObject

Текущая система не имеет механизма для создания комбинируемых спеллов. Новая система должна вписаться в существующую архитектуру без создания параллельных механизмов.

## Goals / Non-Goals

**Goals:**
- Создать расширяемую систему спеллов через композицию компонентов (Activation, Targeting, Impact, Effects)
- Разделить конфигурацию (SpellDefinition) и прогресс игрока (SpellModel)
- Интегрироваться с существующей системой AttributeModifier для модификации параметров
- Поддерживать instant, duration и periodic эффекты без дублирования кода
- Обеспечить сохранение прогресса через существующую DataStorage архитектуру
- Добавить события для UI подписки (OnSpellUnlocked, OnSpellLevelChanged)

**Non-Goals:**
- Переписывание существующей системы оружия (RangedWeapon, ProjectileBase) на Spell system
- Создание второй независимой системы модификаторов
- Создание второй независимой системы ресурсов
- Привязка спеллов к UnitTier
- Изменение существующих UnitModel, EpochModel, InventoryModel

## Decisions

### 1. Разделение SpellDefinition и SpellModel

**Решение:** Использовать паттерн Definition + Model аналогично UnitInfo/UnitModel и EpochInfo/EpochModel.

**Обоснование:**
- SpellDefinition (ScriptableObject) содержит immutable конфигурацию: Id, Name, Icon, Base configuration, MaxLevel, Upgrade configuration
- SpellModel содержит runtime состояние: CurrentLevel, IsUnlocked, CalculatedStats
- Позволяет безопасно пересоздавать модели и менять уровень без мутации ScriptableObject

**Альтернативы рассмотрены:**
- Единый класс с mutable полями → отклонено из-за риска мутации конфигурации
- Полностью отдельная система → отклонено из-за дублирования с существующими паттернами

### 2. Архитектура Effects через базовый класс с lifecycle

**Решение:** Создать абстрактный SpellEffect с методами Apply(), Update(), Expire(), Remove(). Duration и periodic поведение настраивается через параметры, не через наследование.

**Обоснование:**
- DamageEffect с Duration=0 работает как instant
- FreezeEffect с Duration=3 создает состояние на 3 секунды
- BurnEffect с Duration=5 и TickInterval=1 работает как DoT
- Добавление новых эффектов не требует изменения базовой архитектуры

**Альтернативы рассмотрены:**
- Отдельные иерархии InstantEffect/DurationEffect → отклонено из-за дублирования кода
- Отдельная фундаментальная система для DoT → отклонено, DoT это комбинация duration + periodic

### 3. Интеграция с существующей AttributeModifier системой

**Решение:** Расширить существующую систему AttributeModifier для поддержки параметров спеллов (Damage, HealAmount, Duration, Radius, Range, Cooldown, CastTime, EffectStrength, TickInterval).

**Обоснование:**
- Сохраняет существующую семантику (Add, Multiply, Override)
- Параметры спелла модифицируются через Level modifiers
- Base SpellDefinition + Player Level + Level Modifiers = Calculated Runtime Values

**Альтернативы рассмотрены:**
- Создание отдельной системы модификаторов для спеллов → отклонено из-за дублирования

### 4. SpellUpgrade configuration

**Решение:** Хранить конфигурацию изменений по уровням в SpellDefinition как массив SpellUpgrade с полями Level, Cost, Modifiers[].

**Обоснование:**
- Позволяет уровню изменять несколько параметров одновременно
- Конфигурация находится в data, не в UI
- Пример: Level 3 может изменять Damage +50 и Radius +0.5

### 5. Storage через существующую DataStorage архитектуру

**Решение:** Создать SpellProgressStorageData и SpellProgressStorage используя существующие IStorageProvider, StringStorageVariable и Newtonsoft.Json.

**Обоснование:**
- Следует существующему паттерну EpochStorageData
- Хранит только минимум: SpellId, IsUnlocked, Level
- Не сохраняет весь SpellInfo целиком

**Альтернативы рассмотрены:**
- Новый механизм сохранения → отклонено из-за дублирования с DataStorage

### 6. SpellCollection/SpellManager вместо глобального singleton

**Решение:** Создать SpellCollection как domain model с методами GetSpell(), GetAllSpells(), UnlockSpell(), UpgradeSpell(). Интегрировать через DI (Zenject) аналогично MainModel, EpochModel.

**Обоснование:**
- Следует существующему паттерну DI в проекте
- Позволяет тестировать и заменять реализации
- Избегает глобального состояния

### 7. ScriptableObject конфигурация

**Решение:** Создать SpellInfo как ScriptableObject аналогично UnitInfo, EpochInfo, GameStats. Предусмотреть SpellDatabase для управления множеством спеллов.

**Обоснование:**
- Позволяет создавать конфигурации через Unity Inspector
- Следует существующему паттерну проекта
- Легко расширяемо для большого количества спеллов

## Risks / Trade-offs

**Risk:** Сложность интеграции с существующей AttributeModifier системой если она жестко привязана к конкретным атрибутам.
**Mitigation:** Провести ревью существующего кода AttributeModifier перед реализацией. Использовать generic подход где возможно.

**Risk:** Duration эффекты могут создать сложность с управлением lifecycle и очисткой.
**Mitigation:** Четко определить lifecycle (Apply → Active → Expire → Remove). Использовать Unity coroutine или отдельный manager для Update.

**Risk:** Мутация ScriptableObject при прокачке если разработчик ошибется.
**Mitigation:** Явно документировать правило "не мутировать ScriptableObject". Использовать calculated runtime values.

**Trade-off:** Гибкость vs сложность. Композиционная система более гибкая но требует больше классов.
**Mitigation:** Начать с минимального набора (Instant, Unit Targeting, Damage/Freeze effects) и расширять по необходимости.

## Migration Plan

1. Создать базовую архитектуру (SpellDefinition, SpellModel, SpellCollection)
2. Интегрировать с существующей AttributeModifier системой
3. Реализовать Storage для прогресса
4. Создать пример спеллов (Fireball, Freeze) для валидации
5. Добавить Events для UI
6. Тестировать сохранение/загрузку прогресса

**Rollback strategy:** Система изолирована от существующего кода, можно отключить интеграцию без влияния на UnitModel/EpochModel.

## Open Questions

1. Нужно ли создавать отдельный SpellEffectManager для управления duration эффектами или использовать существующий Update механизм?
2. Как именно интегрироваться с существующей combat системой для применения damage?
3. Где хранить SpellDatabase - в Resources или через существующий механизм загрузки ScriptableObject?
