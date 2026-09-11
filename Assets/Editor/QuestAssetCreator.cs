using UnityEngine;
using UnityEditor;
using Core.Application.Quests;
using Core.Application.Requirements.SaveState;
using Core.Application.Requirements.Base;
using Core.Application.Models;
using Core.Application.Info.Reward;
using Unity.Game;

namespace Editor
{
    public class QuestAssetCreator
    {
        [MenuItem("Tools/Quests/Create All Epoch Quests")]
        public static void CreateAllEpochQuests()
        {
            CreateEpoch1Quests();
            CreateEpoch2Quests();
            CreateEpoch3Quests();
            CreateEpoch4Quests();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("All epoch quests created successfully");
        }

        private static void SetResourcesEarnedReq(SerializedProperty requirementProp, ResourceType resourceType, int amount)
        {
            var resourceProp = requirementProp.FindPropertyRelative("_resource");
            resourceProp.FindPropertyRelative("Type").enumValueIndex = (int)resourceType;
            resourceProp.FindPropertyRelative("Value").intValue = amount;
        }

        private static void SetUnitDeadReq(SerializedProperty requirementProp, UnitTier tier, Faction faction, int count)
        {
            requirementProp.FindPropertyRelative("_tier").enumValueIndex = (int)tier;
            requirementProp.FindPropertyRelative("_faction").enumValueIndex = (int)faction;
            requirementProp.FindPropertyRelative("_count").intValue = count;
        }

        private static void SetUnitSpawnedReq(SerializedProperty requirementProp, UnitTier tier, Faction faction, int count)
        {
            requirementProp.FindPropertyRelative("_tier").enumValueIndex = (int)tier;
            requirementProp.FindPropertyRelative("_faction").enumValueIndex = (int)faction;
            requirementProp.FindPropertyRelative("_count").intValue = count;
        }

        private static void SetDamageAppliedReq(SerializedProperty requirementProp, float damage, Faction faction)
        {
            requirementProp.FindPropertyRelative("_appliedDamage").floatValue = damage;
            requirementProp.FindPropertyRelative("_faction").enumValueIndex = (int)faction;
        }

        private static void CreateQuestWithReq(string id, string name, string description, IRequirement requirement, System.Action<SerializedProperty> setReq, Reward reward, string folderPath)
        {
            var quest = ScriptableObject.CreateInstance<QuestItemConfig>();
            var so = new SerializedObject(quest);

            so.FindProperty("_id").stringValue = id;
            so.FindProperty("_name").stringValue = name;
            so.FindProperty("_description").stringValue = description;

            var requirementProp = so.FindProperty("_requirement");
            requirementProp.managedReferenceValue = requirement;
            so.ApplyModifiedProperties();

            setReq(requirementProp);

            so.FindProperty("_reward").boxedValue = reward;
            so.ApplyModifiedProperties();

            AssetDatabase.CreateAsset(quest, $"{folderPath}/{id}.asset");
        }

        [MenuItem("Tools/Quests/Create Epoch 1 Quests")]
        public static void CreateEpoch1Quests()
        {
            var folder = "Assets/Data/Quests/Epoch1";
            var reward = Reward.ResourceReward(ResourceType.Crystal, 100);

            CreateQuestWithReq("epoch1_earn_300_gold", "Заработать 300 золота", "Заработать 300 золота", 
                new ReqResourcesEarned(), p => SetResourcesEarnedReq(p, ResourceType.Money, 300), reward, folder);

            CreateQuestWithReq("epoch1_kill_5_tier1", "Убить 5 вражеских юнитов Tier1", "Убить 5 вражеских юнитов Tier1",
                new ReqUnitDead(), p => SetUnitDeadReq(p, UnitTier.Tier1, Faction.Enemy, 5), reward, folder);

            CreateQuestWithReq("epoch1_spawn_3_tier1", "Заспавнить 3 юнита Tier1", "Заспавнить 3 юнита Tier1",
                new ReqUnitSpawned(), p => SetUnitSpawnedReq(p, UnitTier.Tier1, Faction.Player, 3), reward, folder);

            CreateQuestWithReq("epoch1_damage_200", "Нанести 200 урона врагам", "Нанести 200 урона врагам",
                new ReqDamageApplied(), p => SetDamageAppliedReq(p, 200, Faction.Enemy), reward, folder);

            CreateQuestWithReq("epoch1_kill_3_tier2", "Убить 3 вражеских юнита Tier2", "Убить 3 вражеских юнита Tier2",
                new ReqUnitDead(), p => SetUnitDeadReq(p, UnitTier.Tier2, Faction.Enemy, 3), reward, folder);

            CreateQuestWithReq("epoch1_spawn_2_tier2", "Заспавнить 2 юнита Tier2", "Заспавнить 2 юнита Tier2",
                new ReqUnitSpawned(), p => SetUnitSpawnedReq(p, UnitTier.Tier2, Faction.Player, 2), reward, folder);

            CreateQuestWithReq("epoch1_earn_500_gold", "Заработать 500 золота", "Заработать 500 золота",
                new ReqResourcesEarned(), p => SetResourcesEarnedReq(p, ResourceType.Money, 500), reward, folder);

            CreateQuestWithReq("epoch1_kill_10_any", "Убить 10 вражеских юнитов", "Убить 10 вражеских юнитов (любых тиров)",
                new ReqUnitDead(), p => SetUnitDeadReq(p, UnitTier.Tier1, Faction.Enemy, 10), reward, folder);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Epoch 1 quests created");
        }

        [MenuItem("Tools/Quests/Create Epoch 2 Quests")]
        public static void CreateEpoch2Quests()
        {
            var folder = "Assets/Data/Quests/Epoch2";
            var reward = Reward.ResourceReward(ResourceType.Crystal, 200);

            CreateQuestWithReq("epoch2_earn_2000_gold", "Заработать 2000 золота", "Заработать 2000 золота",
                new ReqResourcesEarned(), p => SetResourcesEarnedReq(p, ResourceType.Money, 2000), reward, folder);

            CreateQuestWithReq("epoch2_kill_8_tier1", "Убить 8 вражеских юнитов Tier1", "Убить 8 вражеских юнитов Tier1",
                new ReqUnitDead(), p => SetUnitDeadReq(p, UnitTier.Tier1, Faction.Enemy, 8), reward, folder);

            CreateQuestWithReq("epoch2_spawn_5_tier1", "Заспавнить 5 юнитов Tier1", "Заспавнить 5 юнитов Tier1",
                new ReqUnitSpawned(), p => SetUnitSpawnedReq(p, UnitTier.Tier1, Faction.Player, 5), reward, folder);

            CreateQuestWithReq("epoch2_damage_1000", "Нанести 1000 урона врагам", "Нанести 1000 урона врагам",
                new ReqDamageApplied(), p => SetDamageAppliedReq(p, 1000, Faction.Enemy), reward, folder);

            CreateQuestWithReq("epoch2_kill_5_tier2", "Убить 5 вражеских юнитов Tier2", "Убить 5 вражеских юнитов Tier2",
                new ReqUnitDead(), p => SetUnitDeadReq(p, UnitTier.Tier2, Faction.Enemy, 5), reward, folder);

            CreateQuestWithReq("epoch2_spawn_3_tier3", "Заспавнить 3 юнита Tier3", "Заспавнить 3 юнита Tier3",
                new ReqUnitSpawned(), p => SetUnitSpawnedReq(p, UnitTier.Tier3, Faction.Player, 3), reward, folder);

            CreateQuestWithReq("epoch2_earn_3000_gold", "Заработать 3000 золота", "Заработать 3000 золота",
                new ReqResourcesEarned(), p => SetResourcesEarnedReq(p, ResourceType.Money, 3000), reward, folder);

            CreateQuestWithReq("epoch2_kill_3_tier3", "Убить 3 юнита Tier3", "Убить 3 юнита Tier3",
                new ReqUnitDead(), p => SetUnitDeadReq(p, UnitTier.Tier3, Faction.Enemy, 3), reward, folder);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Epoch 2 quests created");
        }

        [MenuItem("Tools/Quests/Create Epoch 3 Quests")]
        public static void CreateEpoch3Quests()
        {
            var folder = "Assets/Data/Quests/Epoch3";
            var reward = Reward.ResourceReward(ResourceType.Crystal, 300);

            CreateQuestWithReq("epoch3_earn_15000_gold", "Заработать 15000 золота", "Заработать 15000 золота",
                new ReqResourcesEarned(), p => SetResourcesEarnedReq(p, ResourceType.Money, 15000), reward, folder);

            CreateQuestWithReq("epoch3_kill_10_tier2", "Убить 10 вражеских юнитов Tier2", "Убить 10 вражесских юнитов Tier2",
                new ReqUnitDead(), p => SetUnitDeadReq(p, UnitTier.Tier2, Faction.Enemy, 10), reward, folder);

            CreateQuestWithReq("epoch3_spawn_7_tier2", "Заспавнить 7 юнитов Tier2", "Заспавнить 7 юнитов Tier2",
                new ReqUnitSpawned(), p => SetUnitSpawnedReq(p, UnitTier.Tier2, Faction.Player, 7), reward, folder);

            CreateQuestWithReq("epoch3_damage_4000", "Нанести 4000 урона врагам", "Нанести 4000 урона врагам",
                new ReqDamageApplied(), p => SetDamageAppliedReq(p, 4000, Faction.Enemy), reward, folder);

            CreateQuestWithReq("epoch3_kill_5_tier3", "Убить 5 вражесских юнитов Tier3", "Убить 5 вражесских юнитов Tier3",
                new ReqUnitDead(), p => SetUnitDeadReq(p, UnitTier.Tier3, Faction.Enemy, 5), reward, folder);

            CreateQuestWithReq("epoch3_spawn_5_tier3", "Заспавнить 5 юнитов Tier3", "Заспавнить 5 юнитов Tier3",
                new ReqUnitSpawned(), p => SetUnitSpawnedReq(p, UnitTier.Tier3, Faction.Player, 5), reward, folder);

            CreateQuestWithReq("epoch3_earn_20000_gold", "Заработать 20000 золота", "Заработать 20000 золота",
                new ReqResourcesEarned(), p => SetResourcesEarnedReq(p, ResourceType.Money, 20000), reward, folder);

            CreateQuestWithReq("epoch3_kill_15_any", "Убить 15 вражесских юнитов", "Убить 15 вражесских юнитов (любых тиров)",
                new ReqUnitDead(), p => SetUnitDeadReq(p, UnitTier.Tier1, Faction.Enemy, 15), reward, folder);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Epoch 3 quests created");
        }

        [MenuItem("Tools/Quests/Create Epoch 4 Quests")]
        public static void CreateEpoch4Quests()
        {
            var folder = "Assets/Data/Quests/Epoch4";
            var reward = Reward.ResourceReward(ResourceType.Crystal, 400);

            CreateQuestWithReq("epoch4_earn_100000_gold", "Заработать 100000 золота", "Заработать 100000 золота",
                new ReqResourcesEarned(), p => SetResourcesEarnedReq(p, ResourceType.Money, 100000), reward, folder);

            CreateQuestWithReq("epoch4_kill_12_tier2", "Убить 12 вражесских юнитов Tier2", "Убить 12 вражесских юнитов Tier2",
                new ReqUnitDead(), p => SetUnitDeadReq(p, UnitTier.Tier2, Faction.Enemy, 12), reward, folder);

            CreateQuestWithReq("epoch4_spawn_8_tier2", "Заспавнить 8 юнитов Tier2", "Заспавнить 8 юнитов Tier2",
                new ReqUnitSpawned(), p => SetUnitSpawnedReq(p, UnitTier.Tier2, Faction.Player, 8), reward, folder);

            CreateQuestWithReq("epoch4_damage_12000", "Нанести 12000 урона врагам", "Нанести 12000 урона врагам",
                new ReqDamageApplied(), p => SetDamageAppliedReq(p, 12000, Faction.Enemy), reward, folder);

            CreateQuestWithReq("epoch4_kill_7_tier3", "Убить 7 вражесских юнитов Tier3", "Убить 7 вражесских юнитов Tier3",
                new ReqUnitDead(), p => SetUnitDeadReq(p, UnitTier.Tier3, Faction.Enemy, 7), reward, folder);

            CreateQuestWithReq("epoch4_spawn_10_tier3", "Заспавнить 10 юнитов Tier3", "Заспавнить 10 юнитов Tier3",
                new ReqUnitSpawned(), p => SetUnitSpawnedReq(p, UnitTier.Tier3, Faction.Player, 10), reward, folder);

            CreateQuestWithReq("epoch4_earn_150000_gold", "Заработать 150000 золота", "Заработать 150000 золота",
                new ReqResourcesEarned(), p => SetResourcesEarnedReq(p, ResourceType.Money, 150000), reward, folder);

            CreateQuestWithReq("epoch4_kill_20_any", "Убить 20 вражесских юнитов", "Убить 20 вражесских юнитов (любых тиров)",
                new ReqUnitDead(), p => SetUnitDeadReq(p, UnitTier.Tier1, Faction.Enemy, 20), reward, folder);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Epoch 4 quests created");
        }
    }
}
