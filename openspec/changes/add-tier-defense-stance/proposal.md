## Why

В игре отсутствует механика оборонительной стойки для юнитов определенного тира, что ограничивает тактические возможности игрока. Добавление этой механики позволит:
- Создавать новые стратегические сценарии через оборонительные позиции
- Увеличить глубину progression-системы через покупку способности за кристаллы
- Дать игроку больше контроля над поведением юнитов во время боя

## What Changes

- **EpochModel.OpenDefenseStance**: новый метод в EpochModel для покупки оборонительной стойки за кристаллы (по аналогии с OpenUnit)
- **UnitModel.DefenseStance**: добавление свойства и методов для управления оборонительной стойкой в UnitModel
- **UnitEntity.DefenseStanceCost**: добавление атрибута стоимости оборонительной стойки в UnitEntity (аналогично UnlockCost)
- **GameStats.GetDefenseStanceCost**: новый метод в GameStats для расчета стоимости способности по эпохе и тиру
- **DefenseStanceState**: новый state machine state для юнитов в оборонительной стойке (idle на месте, атака при приближении врага)
- **TierDefenseStanceUI**: UI элемент в Unity.Presentation для активации/деактивации способности во время боя
- **Интеграция с UnitController**: добавление логики переключения между обычным движением и оборонительной стойкой

## Capabilities

### New Capabilities
- `tier-defense-stance`: механика оборонительной стойки для юнитов определенного тира с возможностью активации/деактивации во время боя
- `defense-stance-purchase`: покупка оборонительной стойки за кристаллы через EpochModel (по аналогии с OpenUnit)
- `defense-stance-state`: state machine state для поведения юнитов в обороне (стойка на месте, атака в радиусе, возврат в idle)

### Modified Capabilities
- `unit-behavior`: расширение state machine для поддержки оборонительной стойки (без изменения требований, только добавление нового state)
- `epoch-model`: добавление метода OpenDefenseStance для покупки способности

## Impact

- **Core.Application**: EpochModel расширен методом OpenDefenseStance, UnitModel добавлены свойства DefenseStance, UnitEntity добавлен атрибут DefenseStanceCost, GameStats добавлен метод GetDefenseStanceCost
- **Core.Application.DataStorage**: EpochStorageData расширен для хранения состояния покупки оборонительной стойки
- **Unity.Game**: UnitController расширен для поддержки переключения в оборонительную стойку
- **Unity.Presentation**: добавлен TierDefenseStanceUI для управления способностью
