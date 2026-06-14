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
        private VisualElement _unitTierTabsContent;
        private VisualElement _unitTier1Container;
        private VisualElement _unitTier2Container;
        private VisualElement _unitTier3Container;
        private VisualElement _unitTier1FieldContainer;
        private VisualElement _unitTier2FieldContainer;
        private VisualElement _unitTier3FieldContainer;
        
        private SerializedObject _serializedObject;
        private SerializedProperty _epochsProperty;
        
        private int _selectedEpochIndex = -1;
        private int _selectedUnitTierIndex = 0;
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
            _unitTierTabsContent = _root.Q<VisualElement>("unit-tier-tabs-content");
            _unitTier1Container = _root.Q<VisualElement>("unit-tier1-container");
            _unitTier2Container = _root.Q<VisualElement>("unit-tier2-container");
            _unitTier3Container = _root.Q<VisualElement>("unit-tier3-container");
            _unitTier1FieldContainer = _root.Q<VisualElement>("unit-tier1-field-container");
            _unitTier2FieldContainer = _root.Q<VisualElement>("unit-tier2-field-container");
            _unitTier3FieldContainer = _root.Q<VisualElement>("unit-tier3-field-container");
            
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
            
            var towerPrefabProperty = epochProperty.FindPropertyRelative("_tower._towerPrefab._value");
            var towerField = _root.Q<PropertyField>("tower-field");
            towerField.BindProperty(towerPrefabProperty);
            
            var wavesField = _root.Q<PropertyField>("waves-field");
            wavesField.BindProperty(epochProperty.FindPropertyRelative("_waves"));
            
            RefreshUnitTierTabs();
            ShowUnitTierFields(_selectedUnitTierIndex);
        }
        
        private void RefreshUnitTierTabs()
        {
            _unitTierTabsContent.Clear();
            
            var tiers = new[] { "Tier 1", "Tier 2", "Tier 3" };
            
            for (int i = 0; i < tiers.Length; i++)
            {
                var tab = new Button
                {
                    text = tiers[i],
                    name = $"unit-tier-tab-{i}"
                };
                
                tab.AddToClassList("epoch-tab");
                
                if (i == _selectedUnitTierIndex)
                {
                    tab.AddToClassList("selected");
                }
                
                var capturedIndex = i;
                tab.clicked += () =>
                {
                    _selectedUnitTierIndex = capturedIndex;
                    RefreshUnitTierTabs();
                    ShowUnitTierFields(capturedIndex);
                };
                
                _unitTierTabsContent.Add(tab);
            }
        }
        
        private void ShowUnitTierFields(int tierIndex)
        {
            _unitTier1Container.style.display = DisplayStyle.None;
            _unitTier2Container.style.display = DisplayStyle.None;
            _unitTier3Container.style.display = DisplayStyle.None;
            
            if (_selectedEpochIndex < 0 || _selectedEpochIndex >= _epochsProperty.arraySize)
                return;
            
            var epochProperty = _epochsProperty.GetArrayElementAtIndex(_selectedEpochIndex);
            
            switch (tierIndex)
            {
                case 0:
                    _unitTier1Container.style.display = DisplayStyle.Flex;
                    _unitTier1FieldContainer.Clear();
                    var unitTier1Property = epochProperty.FindPropertyRelative("_unitTier1");
                    AddUnitInfoFields(_unitTier1FieldContainer, unitTier1Property);
                    
                    var unitTier1ModifiersContainer = _root.Q<VisualElement>("unit-tier1-modifiers-container");
                    var unitTier1ModifiersProperty = epochProperty.FindPropertyRelative("_unitTier1Modifiers");
                    AddModifiersList(unitTier1ModifiersContainer, unitTier1ModifiersProperty);
                    break;
                case 1:
                    _unitTier2Container.style.display = DisplayStyle.Flex;
                    _unitTier2FieldContainer.Clear();
                    var unitTier2Property = epochProperty.FindPropertyRelative("_unitTier2");
                    AddUnitInfoFields(_unitTier2FieldContainer, unitTier2Property);
                    
                    var unitTier2ModifiersContainer = _root.Q<VisualElement>("unit-tier2-modifiers-container");
                    var unitTier2ModifiersProperty = epochProperty.FindPropertyRelative("_unitTier2Modifiers");
                    AddModifiersList(unitTier2ModifiersContainer, unitTier2ModifiersProperty);
                    break;
                case 2:
                    _unitTier3Container.style.display = DisplayStyle.Flex;
                    _unitTier3FieldContainer.Clear();
                    var unitTier3Property = epochProperty.FindPropertyRelative("_unitTier3");
                    AddUnitInfoFields(_unitTier3FieldContainer, unitTier3Property);
                    
                    var unitTier3ModifiersContainer = _root.Q<VisualElement>("unit-tier3-modifiers-container");
                    var unitTier3ModifiersProperty = epochProperty.FindPropertyRelative("_unitTier3Modifiers");
                    AddModifiersList(unitTier3ModifiersContainer, unitTier3ModifiersProperty);
                    break;
            }
        }
        
        private void AddUnitInfoFields(VisualElement container, SerializedProperty unitInfoProperty)
        {
            var nameField = new PropertyField(unitInfoProperty.FindPropertyRelative("_name"), "Name");
            nameField.BindProperty(unitInfoProperty.FindPropertyRelative("_name"));
            container.Add(nameField);
            
            var tierField = new PropertyField(unitInfoProperty.FindPropertyRelative("_tier"), "Tier");
            tierField.BindProperty(unitInfoProperty.FindPropertyRelative("_tier"));
            container.Add(tierField);
            
            var prefabField = new PropertyField(unitInfoProperty.FindPropertyRelative("_unitPrefab._value"), "Prefab");
            prefabField.BindProperty(unitInfoProperty.FindPropertyRelative("_unitPrefab._value"));
            container.Add(prefabField);
        }
        
        private void AddModifiersList(VisualElement container, SerializedProperty modifiersProperty)
        {
            container.Clear();
            
            var propertyField = new PropertyField(modifiersProperty);
            propertyField.BindProperty(modifiersProperty);
            container.Add(propertyField);
        }
        
        private void HideEpochFields()
        {
            _noEpochLabel.style.display = DisplayStyle.Flex;
            _epochFieldsContainer.style.display = DisplayStyle.None;
        }
    }
}
