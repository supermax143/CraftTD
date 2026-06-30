using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Unity.Game;

namespace Editor.ChronologyEditor
{
    /// <summary>
    /// Кастомное поле для отображения AssetReference с превью
    /// </summary>
    public class AssetReferencePreviewField : VisualElement
    {
        private readonly PropertyField _propertyField;
        private readonly Image _previewImage;
        private SerializedProperty _property;

        public AssetReferencePreviewField(string label)
        {
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Center;
            style.marginTop = 5;
            style.marginBottom = 5;

            _previewImage = new Image();
            _previewImage.style.width = 64;
            _previewImage.style.height = 64;
            _previewImage.style.minWidth = 64;
            _previewImage.style.minHeight = 64;
            _previewImage.style.marginRight = 10;
            _previewImage.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);

            _propertyField = new PropertyField();
            _propertyField.style.flexGrow = 1;

            _propertyField.RegisterCallback<SerializedPropertyChangeEvent>(OnPropertyChanged);

            Add(_previewImage);
            Add(_propertyField);
        }

        public void BindProperty(SerializedProperty property)
        {
            _property = property;
            _propertyField.BindProperty(property);
            UpdatePreview();
        }

        private void OnPropertyChanged(SerializedPropertyChangeEvent evt)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            if (_property == null)
            {
                _previewImage.image = null;
                return;
            }

            AssetReference assetReference = null;
            
            // Try to get AssetReference from the serialized object
            var targetObject = _property.serializedObject.targetObject;
            if (targetObject != null)
            {
                var propertyPath = _property.propertyPath;
                var unitInfo = GetNestedPropertyValue(targetObject, propertyPath) as UnitInfo;
                if (unitInfo != null)
                {
                    assetReference = unitInfo.UnitPrefab;
                }
            }
            
            if (assetReference == null || !assetReference.RuntimeKeyIsValid())
            {
                _previewImage.image = null;
                return;
            }

#if UNITY_EDITOR
            var editorAsset = assetReference.editorAsset;
            if (editorAsset == null)
            {
                _previewImage.image = null;
                return;
            }

            var preview = AssetPreview.GetAssetPreview(editorAsset);
            if (preview != null)
            {
                _previewImage.image = preview;
            }
#endif
        }
        
        private object GetNestedPropertyValue(object obj, string propertyPath)
        {
            if (obj == null || string.IsNullOrEmpty(propertyPath))
                return null;

            var parts = propertyPath.Split('.');
            object current = obj;

            foreach (var part in parts)
            {
                if (current == null)
                    return null;

                var type = current.GetType();
                var field = type.GetField(part, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                
                if (field == null)
                    return null;

                current = field.GetValue(current);
            }

            return current;
        }
    }
}
