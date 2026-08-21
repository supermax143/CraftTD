## 1. Core.Application - EpochModel.OpenDefenseStance

- [x] 1.1 Добавить метод OpenDefenseStance(UnitTier tier) в EpochModel по аналогии с OpenUnit
- [x] 1.2 Добавить проверку разблокировки юнита тира перед покупкой оборонительной стойки
- [x] 1.3 Добавить проверку достаточности кристаллов перед покупкой
- [x] 1.4 Добавить списание кристаллов при успешной покупке
- [x] 1.5 Добавить событие OnDefenseStanceOpened для уведомления о покупке способности
- [x] 1.6 Добавить сохранение состояния покупки в EpochStorageData

## 2. Core.Application - UnitModel.DefenseStance

- [x] 2.1 Добавить свойство IsDefenseStanceOpened в UnitModel
- [x] 2.2 Добавить свойство DefenseStanceCost в UnitModel (возвращает значение из UnitEntity)
- [x] 2.3 Добавить метод OpenDefenseStance() в UnitModel для изменения состояния
- [x] 2.4 Обновить конструктор UnitModel для инициализации состояния оборонительной стойки из EpochStorageData

## 3. Core.Application - UnitEntity.DefenseStanceCost

- [x] 3.1 Создать класс DefenseStanceCostAttribute по аналогии с UnitUnlockCostAttribute
- [x] 3.2 Добавить поле _defenseStanceCost типа DefenseStanceCostAttribute в UnitEntity
- [x] 3.3 Инициализировать _defenseStanceCost в конструкторе UnitEntity через GameStats.GetDefenseStanceCost
- [x] 3.4 Добавить свойство DefenseStanceCost в UnitEntity для доступа к значению

## 4. Core.Application - GameStats.GetDefenseStanceCost

- [x] 4.1 Добавить метод GetDefenseStanceCost(int epoch, UnitTier tier) в GameStats
- [x] 4.2 Реализовать логику расчета стоимости на основе эпохи и тира (аналогично GetUnitOpeningCost)
- [x] 4.3 Добавить баланс-конфигурацию для стоимости оборонительной стойки

## 5. Core.Application.DataStorage - EpochStorageData

- [x] 5.1 Добавить метод IsDefenseStanceOpened(UnitTier tier) в EpochStorageData
- [x] 5.2 Добавить метод OpenDefenseStance(UnitTier tier) в EpochStorageData для сохранения состояния
- [x] 5.3 Добавить хранение состояния оборонительной стойки в структуре данных EpochStorageData

## 6. Unity.Presentation - TierDefenseStanceUI

- [x] 6.1 Создать класс TierDefenseStanceUI для управления UI оборонительной стойки
- [x] 6.2 Добавить инъекцию EpochModel в UI
- [x] 6.3 Реализовать отображение состояния (куплено/не куплено, активно/не активно)
- [x] 6.4 Добавить обработчик нажатия для покупки способности через EpochModel.OpenDefenseStance
- [x] 6.5 Добавить обработчик нажатия для активации/деактивации стойки
- [x] 6.6 Реализовать подписку на OnDefenseStanceOpened для обновления UI

## 7. Unity.Installers - Биндинги

- [x] 7.1 Убедиться что EpochModel уже забинжен через существующий инсталлер
- [x] 7.2 Проверить что UnitModel доступен через EpochModel.TryGetUnitModel

## 8. Unity.Game - DefenseStanceState

- [x] 8.1 Создать класс DefenseStanceState наследуемый от существующего базового state
- [x] 8.2 Реализовать логику idle анимации в DefenseStanceState
- [x] 8.3 Добавить проверку врагов в радиусе атаки в DefenseStanceState
- [x] 8.4 Реализовать переход в attack state при обнаружении врага в радиусе
- [x] 8.5 Реализовать возврат в idle после смерти врага
- [x] 8.6 Добавить метод ExitDefenseStance для выхода из состояния

## 9. Unity.Game - UnitController интеграция

- [x] 9.1 Добавить инъекцию EpochModel в UnitController
- [x] 9.2 Добавить подписку на событие активации/деактивации оборонительной стойки
- [x] 9.3 Реализовать переключение в DefenseStanceState при активации стойки для тира юнита
- [x] 9.4 Реализовать возврат в обычное состояние при деактивации стойки
- [x] 9.5 Добавить проверку тира юнита для применения стойки
