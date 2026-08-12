using Core.Application.Info.Shop;
using Core.Application.Models;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ShopItemConfig))]
    public class ShopItemConfigDrawer : UnityEditor.Editor
    {
        private SerializedProperty _idProperty;
        private SerializedProperty _nameProperty;
        private SerializedProperty _descriptionProperty;
        private SerializedProperty _prefabProperty;
        private SerializedProperty _paymentTypeProperty;
        private SerializedProperty _currencyTypeProperty;
        private SerializedProperty _priceProperty;
        private SerializedProperty _isConsumableProperty;
        private SerializedProperty _rewardsProperty;

        private void OnEnable()
        {
            _idProperty = serializedObject.FindProperty("_id");
            _nameProperty = serializedObject.FindProperty("_name");
            _descriptionProperty = serializedObject.FindProperty("_description");
            _prefabProperty = serializedObject.FindProperty("_prefab");
            _paymentTypeProperty = serializedObject.FindProperty("_paymentType");
            _currencyTypeProperty = serializedObject.FindProperty("_currencyType");
            _priceProperty = serializedObject.FindProperty("_price");
            _isConsumableProperty = serializedObject.FindProperty("_isConsumable");
            _rewardsProperty = serializedObject.FindProperty("_rewards");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_idProperty);
            EditorGUILayout.PropertyField(_nameProperty);
            EditorGUILayout.PropertyField(_descriptionProperty);
            EditorGUILayout.PropertyField(_prefabProperty);
            EditorGUILayout.PropertyField(_paymentTypeProperty);

            PaymentType paymentType = (PaymentType)_paymentTypeProperty.enumValueIndex;

            if (paymentType == PaymentType.GameCurrency)
            {
                EditorGUILayout.PropertyField(_currencyTypeProperty);
            }

            EditorGUILayout.PropertyField(_priceProperty);
            EditorGUILayout.PropertyField(_isConsumableProperty);
            EditorGUILayout.PropertyField(_rewardsProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
