using Core.Application.Info.Shop;
using Core.Application.Models;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(Reward))]
    public class RewardDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var rewardTypeProperty = property.FindPropertyRelative("_rewardType");
            var resourceTypeProperty = property.FindPropertyRelative("_resourceType");
            var itemTypeProperty = property.FindPropertyRelative("_itemType");
            var countProperty = property.FindPropertyRelative("_count");

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            Rect rewardTypeRect = new Rect(position.x, position.y, position.width, lineHeight);
            Rect resourceTypeRect = new Rect(position.x, position.y + lineHeight + spacing, position.width, lineHeight);
            Rect itemTypeRect = new Rect(position.x, position.y + lineHeight + spacing, position.width, lineHeight);
            Rect countRect = new Rect(position.x, position.y + lineHeight * 2 + spacing * 2, position.width, lineHeight);

            EditorGUI.PropertyField(rewardTypeRect, rewardTypeProperty);

            RewardType rewardType = (RewardType)rewardTypeProperty.enumValueIndex;

            if (rewardType == RewardType.Resource)
            {
                EditorGUI.PropertyField(resourceTypeRect, resourceTypeProperty);
                EditorGUI.PropertyField(countRect, countProperty);
            }
            else if (rewardType == RewardType.Item)
            {
                EditorGUI.PropertyField(itemTypeRect, itemTypeProperty);
                EditorGUI.PropertyField(countRect, countProperty);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var rewardTypeProperty = property.FindPropertyRelative("_rewardType");
            RewardType rewardType = (RewardType)rewardTypeProperty.enumValueIndex;

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            return lineHeight * 3 + spacing * 2;
        }
    }
}
