using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.ChronologyEditor
{
    /// <summary>
    /// Кастомное поле для отображения GameObject с превью
    /// </summary>
    public class GameObjectPreviewField : VisualElement
    {
        private readonly ObjectField _objectField;
        private readonly Image _previewImage;
        private SerializedProperty _property;

        public GameObjectPreviewField(string label)
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

            _objectField = new ObjectField(label);
            _objectField.objectType = typeof(GameObject);
            _objectField.style.flexGrow = 1;

            _objectField.RegisterValueChangedCallback(OnObjectChanged);

            Add(_previewImage);
            Add(_objectField);
        }

        public void BindProperty(SerializedProperty property)
        {
            _property = property;
            _objectField.BindProperty(property);
            UpdatePreview();
        }

        private void OnObjectChanged(ChangeEvent<Object> evt)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            if (_objectField.value == null)
            {
                _previewImage.image = null;
                return;
            }

            var gameObject = _objectField.value as GameObject;
            if (gameObject == null)
            {
                _previewImage.image = null;
                return;
            }

            var preview = AssetPreview.GetAssetPreview(gameObject);
            if (preview != null)
            {
                _previewImage.image = preview;
            }
        }
    }
}
