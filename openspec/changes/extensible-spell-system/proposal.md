## Why

Проект требует расширяемую систему спеллов для добавления новых игровых механик без дублирования кода. Текущая архитектура не имеет механизма для создания комбинируемых эффектов (damage, freeze, burn) с различными способами активации и выбора цели. Система нужна для поддержки разнообразных спеллов (Fireball, Freeze, Burn) через композицию компонентов, а не через создание отдельных классов для каждого спелла.

## What Changes

- **Создание модели спелла**: Разделение на SpellDefinition (конфигурация) и SpellModel (прогресс игрока)
- **Система Activation**: Механизмы активации (Instant, CastTime, Channel, Trigger, Toggle)
- **Система Targeting**: Механизмы выбора цели (Unit, Field, Area, Direction, Point, Cone, Line)
- **Система Impact**: Применение одного или нескольких эффектов к выбранным целям
- **Система Effects**: Базовая архитектура эффектов с поддержкой instant, duration и periodic execution
- **Интеграция с AttributeModifier**: Расширение существующей системы модификаторов для параметров спеллов
- **Система прогресса**: Unlock/Upgrade спеллов с cost через ресурсы
- **Storage**: Сохранение прогресса спеллов через существующую DataStorage архитектуру
- **Events**: События для UI (OnSpellUnlocked, OnSpellLevelChanged)

## Capabilities

### New Capabilities
- `spell-definition`: Конфигурация спелла через ScriptableObject (Id, Name, Icon, Base configuration, MaxLevel)
- `spell-progression`: Прогресс игрока (IsUnlocked, Level, UnlockCost, UpgradeCost)
- `spell-activation`: Механизмы когда и как спелл начинает выполнение
- `spell-targeting`: Механизмы выбора целей для Impact
- `spell-impact`: Применение эффектов к выбранным целям
- `spell-effect`: Базовая система эффектов с lifecycle (Apply, Update, Expire, Remove)
- `spell-modifier`: Интеграция с существующей системой AttributeModifier для параметров спеллов
- `spell-storage`: Сохранение и загрузка прогресса спеллов через DataStorage

### Modified Capabilities
- `resource-entity`: Использование существующих ресурсов (Money или Crystall) для стоимости unlock/upgrade спеллов

## Impact

- **Новые директории**: `Scripts/Core/Application/Spells/`, `Scripts/Unity/Spells/`
- **Интеграция с существующими системами**: GameStats, AttributeModifier, DataStorage, DI/Zenject
- **ScriptableObject конфигурации**: SpellInfo/SpellDefinition аналогично UnitInfo, EpochInfo
- **Storage**: SpellProgressStorage для сохранения прогресса
- **Events**: Добавление событий в SpellModel для UI подписки
- **Без breaking changes**: Существующие UnitModel, EpochModel, RangedWeapon продолжают работать
