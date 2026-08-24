## 1. Изучение существующей архитектуры

- [x] 1.1 Изучить существующую систему AttributeModifier (GameEntityAttribute, AttributeModifierBase)
- [x] 1.2 Изучить существующую систему DataStorage (IStorageProvider, StringStorageVariable)
- [x] 1.3 Изучить существующие UnitInfo/UnitModel и EpochInfo/EpochModel паттерны
- [x] 1.4 Изучить существующую систему GameStats и ScriptableObject конфигурации
- [x] 1.5 Изучить существующую DI систему (Zenject) и интеграцию MainModel, EpochModel

## 2. Создание базовой архитектуры спеллов

- [x] 2.1 Создать директории Scripts/Core/Application/Spells/ и Scripts/Unity/Spells/
- [x] 2.2 Создать SpellDefinition ScriptableObject с полями Id, Name, Description, Icon, MaxLevel
- [x] 2.3 Создать SpellActivationType enum (Instant, CastTime, Channel, Trigger, Toggle)
- [x] 2.4 Создать SpellTargetingType enum (NoTarget, Unit, Field, Area, Direction, Self, Point, Cone, Line)
- [x] 2.5 Создать SpellUpgrade класс с полями Level, Cost, Modifiers[]
- [x] 2.6 Добавить Activation, Targeting, Impact конфигурацию в SpellDefinition
- [x] 2.7 Создать SpellModel с полями CurrentLevel, IsUnlocked, CalculatedStats
- [x] 2.8 Реализовать расчет runtime значений из base configuration + level modifiers

## 3. Система Effects

- [x] 3.1 Создать абстрактный SpellEffect базовый класс с методами Apply(), Update(), Expire(), Remove()
- [x] 3.2 Создать DamageEffect с параметром damage
- [x] 3.3 Создать HealEffect с параметром healAmount
- [x] 3.4 Создать FreezeEffect с параметрами duration, movementSpeedMultiplier
- [x] 3.5 Реализовать instant execution для эффектов с Duration=0
- [x] 3.6 Реализовать duration execution для эффектов с Duration>0
- [x] 3.7 Реализовать periodic execution с TickInterval параметром
- [x] 3.8 Создать SpellImpact класс для применения эффектов к целям

## 4. Система Activation

- [x] 4.1 Создать базовый SpellActivation класс
- [x] 4.2 Реализовать InstantActivation
- [x] 4.3 Реализовать CastTimeActivation с параметром castTime
- [x] 4.4 Реализовать ChannelActivation с параметром channelDuration
- [x] 4.5 Реализовать TriggerActivation
- [x] 4.6 Реализовать ToggleActivation

## 5. Система Targeting

- [x] 5.1 Создать базовый SpellTargeting класс
- [x] 5.2 Реализовать UnitTargeting
- [x] 5.3 Реализовать FieldTargeting с параметром radius
- [x] 5.4 Реализовать AreaTargeting
- [x] 5.5 Реализовать DirectionTargeting с параметрами direction, range
- [x] 5.6 Реализовать SelfTargeting
- [x] 5.7 Реализовать PointTargeting
- [x] 5.8 Реализовать ConeTargeting с параметрами angle, range
- [x] 5.9 Реализовать LineTargeting с параметрами length, width

## 6. Интеграция с AttributeModifier

- [x] 6.1 Провести ревью существующей AttributeModifier системы
- [x] 6.2 Определить какие параметры спеллов будут модифицируемыми
- [x] 6.3 Расширить AttributeModifier для поддержки параметров спеллов (Damage, HealAmount, Duration, Radius, Range, Cooldown, CastTime, EffectStrength, TickInterval)
- [x] 6.4 Реализовать применение level modifiers к runtime значениям
- [x] 6.5 Обеспечить поддержку Add, Multiply, Override семантики для параметров спеллов
- [x] 6.6 Убедиться что ScriptableObject не мутируется при применении модификаторов

## 7. Система прогресса и SpellCollection

- [x] 7.1 Создать SpellProgressData класс с полями SpellId, IsUnlocked, Level
- [x] 7.2 Создать SpellModel с методами Unlock(), Upgrade()
- [x] 7.3 Реализовать проверку ресурсов для unlock/upgrade
- [x] 7.4 Создать SpellCollection с методами GetSpell(), GetAllSpells(), UnlockSpell(), UpgradeSpell()
- [x] 7.5 Интегрировать SpellCollection с DI (Zenject)
- [x] 7.6 Добавить события OnSpellUnlocked, OnSpellLevelChanged в SpellModel

## 8. Storage для прогресса

- [x] 8.1 Создать SpellProgressStorageData класс
- [x] 8.2 Создать SpellProgressStorage используя IStorageProvider и StringStorageVariable
- [x] 8.3 Реализовать сериализацию/десериализацию через Newtonsoft.Json
- [x] 8.4 Реализовать сохранение прогресса спеллов (SpellId, IsUnlocked, Level)
- [x] 8.5 Реализовать загрузку прогресса спеллов
- [x] 8.6 Убедиться что сохраняется только минимальные данные, не весь SpellDefinition
- [ ] 8.7 Протестировать восстановление прогресса после перезапуска

## 9. ScriptableObject конфигурация

- [x] 9.1 Создать SpellInfo ScriptableObject аналогично UnitInfo
- [x] 9.2 Создать SpellDatabase ScriptableObject для управления множеством спеллов
- [x] 9.3 Настроить загрузку SpellDatabase (Resources или существующий механизм)
- [ ] 9.4 Создать пример SpellDefinition для Fireball (требует Unity Editor)
- [ ] 9.5 Создать пример SpellDefinition для Freeze (требует Unity Editor)

## 10. Примеры спеллов для валидации

- [ ] 10.1 Создать Fireball спелл (Instant activation, Unit targeting, DamageEffect) (требует Unity Editor)
- [ ] 10.2 Создать Freeze спелл (Instant activation, Unit targeting, FreezeEffect) (требует Unity Editor)
- [x] 10.3 Создать Burn спелл (Instant activation, Unit targeting, BurnEffect с periodic execution)
- [ ] 10.4 Протестировать что Fireball работает как instant damage
- [ ] 10.5 Протестировать что Freeze работает как duration effect
- [ ] 10.6 Протестировать что Burn работает как periodic DoT

## 11. Интеграция с Resource системой

- [x] 11.1 Использовать существующие ресурсы (Money или Crystall) для стоимости unlock/upgrade спеллов
- [x] 11.2 Реализовать проверку достаточности ресурсов при unlock
- [x] 11.3 Реализовать списание ресурсов при unlock
- [x] 11.4 Реализовать проверку достаточности ресурсов при upgrade
- [x] 11.5 Реализовать списание ресурсов при upgrade
- [ ] 11.6 Протестировать unlock/upgrade с ресурсами

## 12. Тестирование и валидация

- [ ] 12.1 Протестировать что существующие UnitModel, EpochModel продолжают работать
- [ ] 12.2 Протестировать что существующая система оружия не затронута
- [ ] 12.3 Протестировать сохранение и загрузку прогресса спеллов
- [ ] 12.4 Протестировать что добавление нового Effect не требует изменения существующих спеллов
- [ ] 12.5 Протестировать что level modifiers корректно применяются
- [ ] 12.6 Протестировать что ScriptableObject не мутируется во время игры
- [ ] 12.7 Протестировать события OnSpellUnlocked, OnSpellLevelChanged
