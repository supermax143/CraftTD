using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.CopyAssetBundleFolderUtil
{
	public class CopyFolderUtil : EditorWindow {
		private const string CHOOSE_FOLDER_FROM = "Drag'n'Drop folder FROM";
		private const string CHOOSE_FOLDER_TO = "Drag'n'Drop folder TO";
		private const string CHOOSE_BOTH_FOLDERS = "Надо указать обе директории!";
		private const string COPYING_IS_DONE = "Done!";
		private const string COPY = "Copy!";
	
		private readonly DragAndDropPath _dragAndDropPathFrom = new ();
		private readonly DragAndDropPath _dragAndDropPathTo = new ();

		private bool _isCopyingIsDone;
		private Dictionary<string, string> _guidPairs;

		private readonly List<string> _ignoredExtensions = new(){".png", ".jpg", ".bytes"};

	
		[MenuItem("Assets/CustomUtils/Deep copy folder")]
		public static void StartWindow() {
			var window = GetWindow<CopyFolderUtil>(typeof(CopyFolderUtil));
			window.Show(true);
			var windowSize = new Vector2(450f, 200f);
			window.minSize = window.maxSize = windowSize;
		}

		private void CreateGUI() {
			_isCopyingIsDone = false;
		}

		private void OnGUI() {
			EditorGUILayout.Space(5);
			var guiStyle = GUILayout.Width(300);

			var rectFrom = EditorGUILayout.BeginHorizontal();
			EditorGUILayout.TextField(text:_dragAndDropPathFrom.folderName == string.Empty ? CHOOSE_FOLDER_FROM : _dragAndDropPathFrom.folderName, guiStyle);
			EditorGUILayout.EndHorizontal();
		
			EditorGUILayout.LabelField(CHOOSE_FOLDER_FROM);
			EditorGUILayout.LabelField(_dragAndDropPathFrom.path);

			EditorGUILayout.Space(15);

			var rectTo = EditorGUILayout.BeginHorizontal();
			EditorGUILayout.TextField(text:_dragAndDropPathTo.folderName == string.Empty ? CHOOSE_FOLDER_TO : _dragAndDropPathTo.folderName, guiStyle);
			EditorGUILayout.EndHorizontal();
		
			EditorGUILayout.LabelField(CHOOSE_FOLDER_TO);
			EditorGUILayout.LabelField(_dragAndDropPathTo.path);
		
			EditorGUILayout.Space(10);

			handleDndEvents(rectFrom, rectTo);
		
			if (!isAllPathsSelected()) {
				using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar, guiStyle)) {
					EditorGUILayout.HelpBox(CHOOSE_BOTH_FOLDERS, MessageType.Warning, true);
				}
			}
			else {
				if (!_isCopyingIsDone) {
					if (GUILayout.Button(COPY, new[] { GUILayout.Width(300), GUILayout.Height(37) })) {
						startCopying();
					}
				}
				else {
					EditorGUILayout.HelpBox(COPYING_IS_DONE, MessageType.Info, true);
				}
			}
		}

		private void handleDndEvents(Rect rectFrom, Rect rectTo) {
			var eventType = Event.current.type;
			if (!GUI.enabled && Event.current.rawType == EventType.MouseDown) {
				eventType = Event.current.rawType;
			}

			switch (eventType) {
				case EventType.DragExited:
				{
					if (rectFrom.Contains(Event.current.mousePosition) && GUI.enabled) {
						setFolder(_dragAndDropPathFrom);
					} else if(rectTo.Contains(Event.current.mousePosition) && GUI.enabled) {
						setFolder(_dragAndDropPathTo);
					}

					break;
				}
				case EventType.DragPerform:
				case EventType.DragUpdated: 
				{
					if ((rectFrom.Contains(Event.current.mousePosition) || rectTo.Contains(Event.current.mousePosition)) && GUI.enabled) {
						var objectReferences = DragAndDrop.objectReferences;
						DragAndDrop.visualMode = DragAndDropVisualMode.Link;
						if (eventType == EventType.DragPerform) {
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
				}
			}
		}

		private void setFolder(DragAndDropPath dndPath) {
			var objectReferences = DragAndDrop.objectReferences;
			if (objectReferences.Length > 0) {
				var path = AssetDatabase.GetAssetPath(objectReferences[0]);
				var type = AssetDatabase.GetMainAssetTypeAtPath(path);
				if (type == typeof(DefaultAsset)) {
					dndPath.path = path;
					dndPath.folderName = path.Substring(path.LastIndexOf('/')+1);
				}
			}
		}
	
		private bool isAllPathsSelected() => (_dragAndDropPathTo.folderName != null && _dragAndDropPathFrom.folderName != null) &&
		                                     _dragAndDropPathTo.folderName != _dragAndDropPathFrom.folderName;

		private void startCopying() {
			var currentVersionControl = EditorSettings.externalVersionControl;
			if (currentVersionControl != ExternalVersionControl.Generic) {
				EditorSettings.externalVersionControl = ExternalVersionControl.Generic;
			}
		
			generateGuidPairs();
			copyDirectoryRecursively(_dragAndDropPathFrom.path, _dragAndDropPathTo.path);
			changeGuidesToNew();
		
			AssetDatabase.Refresh();
			EditorSettings.externalVersionControl = currentVersionControl;
		
			_isCopyingIsDone = true;
		}

		private void generateGuidPairs() {
			var metaFiles = getFilesRecursively(_dragAndDropPathFrom.path, (f) => f.EndsWith(".meta"));
			_guidPairs = new Dictionary<string, string>();
			foreach (string metaFile in metaFiles) {
				StreamReader file = new StreamReader(metaFile);
				file.ReadLine();
				string guidLine = file.ReadLine();
				file.Close();
				string originalGuid = guidLine.Substring(6, guidLine.Length - 6);
				string newGuid = GUID.Generate().ToString().Replace("-", "");
				_guidPairs.Add(originalGuid, newGuid);
			}
		}

		private static void copyDirectoryRecursively(string sourceDirName, string destDirName) {
			var dirInfo = new DirectoryInfo(sourceDirName);
			var dirs = dirInfo.GetDirectories();

			if (!Directory.Exists(destDirName)) {
				Directory.CreateDirectory(destDirName);
			}

			var files = dirInfo.GetFiles();
			foreach (FileInfo file in files) {
				string tempPath = Path.Combine(destDirName, file.Name);
				file.CopyTo(tempPath, false);
			}

			foreach (var directoryInfo in dirs) {
				string tempPath = Path.Combine(destDirName, directoryInfo.Name);
				copyDirectoryRecursively(directoryInfo.FullName, tempPath);
			}
		}

		private void changeGuidesToNew() {
			List<string> allFiles = getFilesRecursively(_dragAndDropPathTo.path);
			foreach (string fileToModify in allFiles) {
				if (_ignoredExtensions.Any(fileToModify.EndsWith)) continue;
			
				string content = File.ReadAllText(fileToModify);
				foreach (var guidPair in _guidPairs) {
					content = content.Replace(guidPair.Key, guidPair.Value);
				}

				File.WriteAllText(fileToModify, content);
			}
			_guidPairs.Clear();
		}

		private List<string> getFilesRecursively(string path, Func<string, bool> criteria = null, List<string> files = null) {
			files ??= new List<string>();
			files.AddRange(Directory.GetFiles(path).Where(f => criteria == null || criteria(f)));

			foreach (string directory in Directory.GetDirectories(path)) {
				getFilesRecursively(directory, criteria, files);
			}

			return files;
		}

		private class DragAndDropPath  {
			public string folderName;
			public string path;
		}
	}
}
