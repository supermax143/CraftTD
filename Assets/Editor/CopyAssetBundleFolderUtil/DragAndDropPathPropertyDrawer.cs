using UnityEditor;
using UnityEngine;

namespace Editor.CopyAssetBundleFolderUtil
{
    public partial class CopyAssetBundleFolderUtil {
        [CustomPropertyDrawer(typeof(DragAndDropPathPropertyAttribute))]
        public class DragAndDropPathPropertyDrawer : PropertyDrawer {
            private string _targetPath;

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
                var labelText = property.FindPropertyRelative("labelText").stringValue;
                var path = property.serializedObject.targetObject as DragAndDropPath;
                _targetPath = EditorGUI.TextField(position, labelText, _targetPath);
                path.setPath(_targetPath);
                var eventType = Event.current.type;
                if (!GUI.enabled && Event.current.rawType == EventType.MouseDown) {
                    eventType = Event.current.rawType;
                }

                switch (eventType) {
                    case EventType.DragExited:
                        if (position.Contains(Event.current.mousePosition) && GUI.enabled) {
                            var objectReferences = DragAndDrop.objectReferences;
                            if (objectReferences.Length > 0) {
                                _targetPath = AssetDatabase.GetAssetPath(objectReferences[0]).Remove(0, FolderCopyDialog.ASSET_BUNDLES_PATH.Length + 1);
                            }
                        }
                        break;
                
                    case EventType.DragPerform:
                    case EventType.DragUpdated:
                        if (position.Contains(Event.current.mousePosition) && GUI.enabled) {
                            var objectReferences = DragAndDrop.objectReferences;
                            DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                            if (eventType == EventType.DragPerform) {
                                if (objectReferences.Length > 0) {
                                    _targetPath = objectReferences[0].name;
                                    path.setPath(_targetPath);
                                }
                                GUI.changed = true;
                                DragAndDrop.AcceptDrag();
                                DragAndDrop.activeControlID = 0;
                            }
                            else {
                                DragAndDrop.activeControlID = GUIUtility.GetControlID(FocusType.Passive);
                            }
                        
                            Event.current.Use();
                        }
                        break;
                
                    default:
                        return;
                }
            }
        }
    }
}