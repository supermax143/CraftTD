using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Game;

namespace Editor.ChronologyEditor
{
    /// <summary>
    /// Окно редактора для ChronologyInfo с использованием UI Toolkit
    /// Позволяет редактировать эпохи с табами для переключения между ними
    /// </summary>
    public class ChronologyInfoEditor : EditorWindow
    {
        private VisualElement _root;
        private VisualElement _tabsContent;
        private VisualElement _epochFieldsContainer;
        private Label _noEpochLabel;
        
        private SerializedObject _serializedObject;
        private SerializedProperty _epochsProperty;
        
        private int _selectedEpochIndex = -1;
        private ChronologyInfo _chronologyInfo;
        
        [MenuItem("Assets/Chronology Editor")]
        public static void ShowWindow()
        {
            var selectedObject = Selection.activeObject as ChronologyInfo;
            if (selectedObject == null)
            {
                Debug.LogError("Please select a ChronologyInfo asset first.");
                return;
            }
            
            var window = GetWindow<ChronologyInfoEditor>();
            window.titleContent = new GUIContent("Chronology Editor");
            window._chronologyInfo = selectedObject;
            window.Initialize();
        }
        
        [MenuItem("Assets/Chronology Editor", true)]
        public static bool ValidateShowWindow()
        {
            return Selection.activeObject is ChronologyInfo;
        }
        
        [MenuItem("CONTEXT/ChronologyInfo/Open Editor")]
        public static void ShowWindowFromContext(MenuCommand command)
        {
            var chronologyInfo = command.context as ChronologyInfo;
            if (chronologyInfo == null) return;
            
            var window = GetWindow<ChronologyInfoEditor>();
            window.titleContent = new GUIContent("Chronology Editor");
            window._chronologyInfo = chronologyInfo;
            window.Initialize();
        }
        
        private void Initialize()
        {
            _serializedObject = new SerializedObject(_chronologyInfo);
            _epochsProperty = _serializedObject.FindProperty("_epochs");
            
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/Editor/ChronologyEditor/ChronologyInfoEditor.uxml");
            
            _root = visualTree.CloneTree();
            
            _tabsContent = _root.Q<VisualElement>("tabs-content");
            _epochFieldsContainer = _root.Q<VisualElement>("epoch-fields-container");
            _noEpochLabel = _root.Q<Label>("no-epoch-label");
            
            var addEpochButton = _root.Q<Button>("add-epoch-button");
            addEpochButton.clicked += AddNewEpoch;
            
            var removeEpochButton = _root.Q<Button>("remove-epoch-button");
            removeEpochButton.clicked += RemoveSelectedEpoch;
            
            RefreshTabs();
            
            rootVisualElement.Add(_root);
        }
        
        private void OnGUI()
        {
            if (_chronologyInfo == null)
            {
                Close();
            }
        }
        
        private void AddNewEpoch()
        {
            _serializedObject.Update();
            
            _epochsProperty.arraySize++;
            var newEpoch = _epochsProperty.GetArrayElementAtIndex(_epochsProperty.arraySize - 1);
            
            var epochNameProperty = newEpoch.FindPropertyRelative("_epochName");
            epochNameProperty.stringValue = $"Epoch {_epochsProperty.arraySize}";
            
            _serializedObject.ApplyModifiedProperties();
            
            _selectedEpochIndex = _epochsProperty.arraySize - 1;
            RefreshTabs();
            ShowEpochFields(_selectedEpochIndex);
        }
        
        private void RemoveSelectedEpoch()
        {
            if (_selectedEpochIndex < 0 || _selectedEpochIndex >= _epochsProperty.arraySize)
                return;
            
            _serializedObject.Update();
            
            _epochsProperty.DeleteArrayElementAtIndex(_selectedEpochIndex);
            
            if (_selectedEpochIndex >= _epochsProperty.arraySize)
            {
                _selectedEpochIndex = _epochsProperty.arraySize - 1;
            }
            
            _serializedObject.ApplyModifiedProperties();
            RefreshTabs();
            
            if (_selectedEpochIndex >= 0)
            {
                ShowEpochFields(_selectedEpochIndex);
            }
            else
            {
                HideEpochFields();
            }
        }
        
        private void RefreshTabs()
        {
            _tabsContent.Clear();
            
            for (int i = 0; i < _epochsProperty.arraySize; i++)
            {
                var epoch = _epochsProperty.GetArrayElementAtIndex(i);
                var epochNameProperty = epoch.FindPropertyRelative("_epochName");
                var epochName = epochNameProperty.stringValue;
                
                var tab = new Button
                {
                    text = epochName,
                    name = $"epoch-tab-{i}"
                };
                
                tab.AddToClassList("epoch-tab");
                
                if (i == _selectedEpochIndex)
                {
                    tab.AddToClassList("selected");
                }
                
                var capturedIndex = i;
                tab.clicked += () =>
                {
                    _selectedEpochIndex = capturedIndex;
                    RefreshTabs();
                    ShowEpochFields(capturedIndex);
                };
                
                _tabsContent.Add(tab);
            }
        }
        
        private void ShowEpochFields(int index)
        {
            if (index < 0 || index >= _epochsProperty.arraySize)
            {
                HideEpochFields();
                return;
            }
            
            _noEpochLabel.style.display = DisplayStyle.None;
            _epochFieldsContainer.style.display = DisplayStyle.Flex;
            
            var epochProperty = _epochsProperty.GetArrayElementAtIndex(index);
            
            var epochNameField = _root.Q<TextField>("epoch-name-field");
            epochNameField.BindProperty(epochProperty.FindPropertyRelative("_epochName"));
            epochNameField.RegisterValueChangedCallback(evt =>
            {
                _serializedObject.Update();
                RefreshTabs();
            });
            
            var towerField = _root.Q<PropertyField>("tower-field");
            towerField.BindProperty(epochProperty.FindPropertyRelative("_tower"));
            
            var unitTier1Field = _root.Q<PropertyField>("unit-tier1-field");
            unitTier1Field.BindProperty(epochProperty.FindPropertyRelative("_unitTier1"));
            
            var unitTier2Field = _root.Q<PropertyField>("unit-tier2-field");
            unitTier2Field.BindProperty(epochProperty.FindPropertyRelative("_unitTier2"));
            
            var unitTier3Field = _root.Q<PropertyField>("unit-tier3-field");
            unitTier3Field.BindProperty(epochProperty.FindPropertyRelative("_unitTier3"));
            
            var wavesField = _root.Q<PropertyField>("waves-field");
            wavesField.BindProperty(epochProperty.FindPropertyRelative("_waves"));
            
            var unitTier1ModifiersField = _root.Q<PropertyField>("unit-tier1-modifiers-field");
            unitTier1ModifiersField.BindProperty(epochProperty.FindPropertyRelative("_unitTier1Modifiers"));
            
            var unitTier2ModifiersField = _root.Q<PropertyField>("unit-tier2-modifiers-field");
            unitTier2ModifiersField.BindProperty(epochProperty.FindPropertyRelative("_unitTier2Modifiers"));
            
            var unitTier3ModifiersField = _root.Q<PropertyField>("unit-tier3-modifiers-field");
            unitTier3ModifiersField.BindProperty(epochProperty.FindPropertyRelative("_unitTier3Modifiers"));
        }
        
        private void HideEpochFields()
        {
            _noEpochLabel.style.display = DisplayStyle.Flex;
            _epochFieldsContainer.style.display = DisplayStyle.None;
        }
    }
}
