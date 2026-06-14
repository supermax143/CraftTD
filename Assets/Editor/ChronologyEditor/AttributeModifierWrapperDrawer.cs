using Core.Application.Info.Attributes.AttributeModifiers;
using UnityEditor;
using UnityEngine;
using Unity.Game.Attributes;

namespace Editor.ChronologyEditor
{
    [CustomPropertyDrawer(typeof(AttributeModifierWrapper))]
    public class AttributeModifierWrapperDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            position.height = EditorGUIUtility.singleLineHeight;
            
            var attributeKindProperty = property.FindPropertyRelative("_attributeKind");
            var modifierKindProperty = property.FindPropertyRelative("_modifierKind");
            var valueProperty = property.FindPropertyRelative("_value");
            
            EditorGUI.PropertyField(position, attributeKindProperty, new GUIContent("Attribute Kind"));
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            
            EditorGUI.PropertyField(position, modifierKindProperty, new GUIContent("Modifier Kind"));
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            
            EditorGUI.PropertyField(position, valueProperty, new GUIContent("Value"));
            
            EditorGUI.EndProperty();
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 2;
        }
    }
}
