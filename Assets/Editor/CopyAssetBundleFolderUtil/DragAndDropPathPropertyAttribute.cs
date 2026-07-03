using System;
using UnityEngine;

namespace Editor.CopyAssetBundleFolderUtil
{
    public partial class CopyAssetBundleFolderUtil {
        [Serializable]
        public class DragAndDropPathPropertyAttribute {
            public string labelText;

            public DragAndDropPathPropertyAttribute(string labelText) {
                this.labelText = labelText;
            }
        }
    
        [Serializable]
        public class DragAndDropPath : ScriptableObject {
            public DragAndDropPathPropertyAttribute attribute;
            private string _path;
            public string path => _path;
        
            public void setLabelName(string labelText) {
                attribute = new DragAndDropPathPropertyAttribute(labelText);
            }
        
            public void setPath(string path) {
                _path = path;
            }
        }
    }
}