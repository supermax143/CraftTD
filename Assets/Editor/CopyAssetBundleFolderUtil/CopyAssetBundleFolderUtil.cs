using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Object = UnityEngine.Object;

namespace Editor.CopyAssetBundleFolderUtil
{
	public partial class CopyAssetBundleFolderUtil {
		public class FolderCopyDialog : EditorWindow {
			public const string ASSET_BUNDLES_PATH = "Assets/" + ASSET_BUNDLES;
		
			private const string BUILTIN_RESOURCES = "Resources/unity_builtin_extra";
			private const string BUILTIN_EXTRA_RESOURCES = "Library/unity default resources";

			private const string ASSET_BUNDLES = "AssetBundles/extends/project_library";
			private const string RESOURCES_PATH = "Assets/Resources";
			private const string DEFAULT_UI_MATERIAL = "Default UI Material";
		
			private static readonly Dictionary<string, string> _pathMap = new Dictionary<string, string>();

			[MenuItem("Assets/CustomUtils/Copy folder from AssetBundles")]
			public static void startWindow() {
				var window = (FolderCopyDialog) GetWindow(typeof(FolderCopyDialog));
				window.Show();
			}

			private static bool isBuiltInAsset(string assetPath) {
				return assetPath.Equals(BUILTIN_RESOURCES) || assetPath.Equals(BUILTIN_EXTRA_RESOURCES) || assetPath.Contains(RESOURCES_PATH);
			}

			private DragAndDropPath _inputFolder;
			private SerializedObject _inputFolderSerialized;
			private DragAndDropPath _sourceFolder;
			private SerializedObject _sourceFolderSerialized;
			private readonly HashSet<string> _incorrectAssetsPaths = new HashSet<string>();

			private string targetPath => ASSET_BUNDLES_PATH + '/' + _inputFolder.path;
			private string sourcePath => ASSET_BUNDLES_PATH + '/' + _sourceFolder.path;

			private bool needProcess(Object obj) {
				return obj != null && !isBuiltInAsset(AssetDatabase.GetAssetPath(obj)) && obj.name != DEFAULT_UI_MATERIAL && isFromSourceFolder(AssetDatabase.GetAssetPath(obj));
			}

			private T getNewAsset<T>(T obj) where T : Object {
				string path = convertPath(AssetDatabase.GetAssetPath(obj));
				return AssetDatabase.LoadAssetAtPath<T>(path);
			}

			private string convertPath(string path) {
				string assetPath = getSubPath(path, $"{sourcePath}/");
				return $"{targetPath}/{assetPath}";
			}

			private bool isFromSourceFolder(string path) {
				string[] arr = path.Split('/');
				string withoutAssetBundlePath = path.Remove(0, arr[0].Length + 1 + arr[1].Length + 1);
				bool isSource = withoutAssetBundlePath.StartsWith(_sourceFolder.path);
				bool isInputFolder = withoutAssetBundlePath.StartsWith(_inputFolder.path);
				if (!isSource && arr[1] == ASSET_BUNDLES && !isInputFolder) {
					_incorrectAssetsPaths.Add(path);
				}

				return isSource;
			}

			private void onPlayModeStateChanged(PlayModeStateChange state) {
				updateFolderPath();
			}

			private void updateFolderPath() {
				_sourceFolder = CreateInstance<DragAndDropPath>();
				_sourceFolder.setLabelName("From:");
				_sourceFolderSerialized = new SerializedObject(_sourceFolder);
				_inputFolder = CreateInstance<DragAndDropPath>();
				_inputFolder.setLabelName("To:");
				_inputFolderSerialized = new SerializedObject(_inputFolder);
			}
		
			private void OnEnable() {
				updateFolderPath();
				EditorApplication.playModeStateChanged += onPlayModeStateChanged;
			}

			private void OnDisable() {
				EditorApplication.playModeStateChanged -= onPlayModeStateChanged;
			}

			private void OnGUI() {
				EditorGUILayout.HelpBox("Understands folders in Assets/AssetBundles only! .", MessageType.Warning);
				GUILayout.Space(10);
				EditorGUILayout.PropertyField(_sourceFolderSerialized.FindProperty("attribute"), false);
				EditorGUILayout.PropertyField(_inputFolderSerialized.FindProperty("attribute"), false);
				GUILayout.Space(10);
				EditorGUILayout.HelpBox("Make deep copy of folder with assets.", MessageType.Info);
				EditorGUILayout.HelpBox("If prefab has references to its nested prefabs. The References will be lost. Should be fixed manually.", MessageType.Error);
				if (GUILayout.Button("Deep Copy (All steps at once)")) {
					_incorrectAssetsPaths.Clear();
					createDir();
					AssetDatabase.Refresh();
					shallowCopy();
					Object[] assets = getAssetsAtPath<Object>();
					changeRefsInMaterials(assets.OfType<Material>().ToArray());
					changeRefsInAnimations(assets.OfType<AnimatorController>().ToArray());

					changeRegularInRegular();
					createVariantsCopy();
					changeVariantsInRegular();
					changeRegularInVariants();
					changeVariantsInVariants();
					changeRefsInPrefabs();
					changeScripts();
				
					showIncorrectAssetsDialog();
				}

				GUILayout.Space(20);
				if (GUILayout.Button("Create dir")) {
					createDir();
				}

				if (GUILayout.Button("Shallow Copy")) {
					shallowCopy();
				}

				if (GUILayout.Button("Change textures in materials")) {
					changeRefsInMaterials();
				}

				if (GUILayout.Button("Change animations in anim controllers")) {
					changeRefsInAnimations();
				}

				if (GUILayout.Button("Replace prefabs in prefabs(Refs lost!)")) {
					_incorrectAssetsPaths.Clear();
					changeRegularInRegular();
					showIncorrectAssetsDialog();
				}
			
				if (GUILayout.Button("Create Variants Copy(1 depth variants only!)")) {
					_incorrectAssetsPaths.Clear();
					createVariantsCopy();
					showIncorrectAssetsDialog();
				}
			
				if (GUILayout.Button("Replace Variants in prefabs(Refs lost!)")) {
					_incorrectAssetsPaths.Clear();
					changeVariantsInRegular();
					showIncorrectAssetsDialog();
				}

				if (GUILayout.Button("Replace prefabs in Variants(Refs lost!)")) {
					_incorrectAssetsPaths.Clear();
					changeRegularInVariants();
					showIncorrectAssetsDialog();
				}
			
				if (GUILayout.Button("Replace Variants in Variants(Refs lost!)")) {
					_incorrectAssetsPaths.Clear();
					changeVariantsInVariants();
					showIncorrectAssetsDialog();
				}

				if (GUILayout.Button("Replace Materials, Sprites and Anim")) {
					_incorrectAssetsPaths.Clear();
					changeRefsInPrefabs();
					showIncorrectAssetsDialog();
				}

				if (GUILayout.Button("Change script's references for external prefabs")) {
					changeScripts();
				}

				GUILayout.Space(10);
			}

			private void showIncorrectAssetsDialog() {
				if (_incorrectAssetsPaths.Count < 1) {
					return;
				}
				var wrongPaths = new StringBuilder();
				foreach (string path in _incorrectAssetsPaths) {
					wrongPaths.Append(path + "\n");
				}

				EditorUtility.DisplayDialog("Assets from another folder", wrongPaths.ToString(), "OK");
			}

			private string getSubPath(string fullPath, string subString) {
				if (string.IsNullOrEmpty(subString)) {
					return fullPath;
				}
				//StringComparison.Ordinal <- не обязательно, но для того, чтоб обезопаситься от языковой зависимости
				int startIndex = fullPath.IndexOf(subString, StringComparison.Ordinal);
				
				return fullPath.Remove(startIndex, subString.Length);
			}
		
			/// <summary>
			///  Changes base prefab of prefab variant. Supports 1 depth only!
			/// </summary>
			private void createVariantsCopy() {
				GameObject[] prefabs = getAssetsAtPath<GameObject>().Where(PrefabUtility.IsPartOfVariantPrefab).ToArray();

				foreach (var prefab in prefabs) {
					var oldBase = PrefabUtility.GetCorrespondingObjectFromOriginalSource(prefab);
					if (!needProcess(oldBase)) {
						continue;
					}

					var newBase = getNewAsset(oldBase);
					var newVariant = (GameObject) PrefabUtility.InstantiatePrefab(newBase);
					void copy(GameObject parent1, GameObject parent2) {
						EditorUtility.CopySerialized(parent1, parent2);

						for (int i = 0; i < parent1.transform.childCount; i++) {
							if ((i + 1) > parent2.transform.childCount) {
								Debug.LogError("createVariantsCopy() Error-> variant base source has another hierarchy ->" + AssetDatabase.GetAssetPath(newBase) );
								return;
							}

							var child1 = parent1.transform.GetChild(i);
							var child2 = parent2.transform.GetChild(i);
							copy(child1.gameObject, child2.gameObject);
						}
					}

					copy(prefab, newVariant);
					PrefabUtility.SaveAsPrefabAsset(newVariant, AssetDatabase.GetAssetPath(prefab));
					DestroyImmediate(newVariant);
				}

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}

			/// <summary>
			///  Copy all dir from source folder to target folder
			/// </summary>
			private void createDir() {
				_pathMap.Clear();
				_pathMap.Add(sourcePath, targetPath);

				if (AssetDatabase.IsValidFolder(sourcePath)) {
					Object[] objectsInFolder = getAssetsAtPath<Object>(sourcePath);
					List<string> assetPaths = objectsInFolder.Select(AssetDatabase.GetAssetPath).ToList();
					HashSet<string> newAssetFoldersPaths = new HashSet<string>();
					foreach (var assetPath in assetPaths) {
						var subPath = getSubPath(assetPath, $"{sourcePath}/");
						List<string> folders = subPath.Split('/').ToList();
						folders.RemoveAt(folders.Count - 1);
						var newPath = $"{targetPath}/{string.Join(Path.DirectorySeparatorChar.ToString(), folders)}";
						newAssetFoldersPaths.Add(newPath);
					}

					foreach (string path in newAssetFoldersPaths) {
						Directory.CreateDirectory(path);
					}

					AssetDatabase.Refresh();
					AssetDatabase.SaveAssets();
				}
			}

			/// <summary>
			///  Makes simple shallow Copy
			/// </summary>
			private void shallowCopy() {
				if (AssetDatabase.IsValidFolder(sourcePath)) {
					Object[] objectsInFolder = getAssetsAtPath<Object>(sourcePath);
					string[] sourceAssetsPaths = objectsInFolder.Select(AssetDatabase.GetAssetPath).Where(s => !AssetDatabase.IsValidFolder(s)).ToArray();
					foreach (string path in sourceAssetsPaths) {
						AssetDatabase.CopyAsset(path, convertPath(path));
					}

					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				}
			}

			/// <summary>
			///  Changes textures in materials
			/// </summary>
			/// <param name="materials">Material array if provided. If not takes material from target folder</param>
			private void changeRefsInMaterials(Material[] materials = null) {
				if (materials == null) {
					materials = getAssetsAtPath<Material>();
				}

				foreach (var material in materials) {
					if (material.mainTexture != null) {
						string texturePath = AssetDatabase.GetAssetPath(material.mainTexture);
						texturePath = convertPath(texturePath);
						material.mainTexture = AssetDatabase.LoadAssetAtPath<Texture>(texturePath);
						AssetDatabase.SaveAssets();
					}
				}
			}

			/// <summary>
			///  Changes Animations in Animation Controllers
			/// </summary>
			/// <param name="animatorControllers"></param>
			private void changeRefsInAnimations(AnimatorController[] animatorControllers = null) {
				if (animatorControllers == null) {
					animatorControllers = getAssetsAtPath<AnimatorController>();
				}

				foreach (var animatorController in animatorControllers) {
					for (int i = 0; i < animatorController.layers.Length; i++) {
						for (int j = 0; j < animatorController.layers[i].stateMachine.states.Length; j++) {
							var state = animatorController.layers[i].stateMachine.states[j];
							if (state.state.motion != null) {
								string path = AssetDatabase.GetAssetPath(state.state.motion);
								path = convertPath(path);
								state.state.motion = AssetDatabase.LoadAssetAtPath<Motion>(path);
							}
						}
					}

					AssetDatabase.SaveAssets();
				}
			}

			/// <summary>
			///  Replaces Animation controllers, Materials in Particles system, sprites and materials in Images with their deep copies from
			///  target folder
			/// </summary>
			private void changeRefsInPrefabs() {
				GameObject[] prefabs = getAssetsAtPath<GameObject>();

				foreach (var prefab in prefabs) {
					traversePrefab(prefab);
					PrefabUtility.SavePrefabAsset(prefab);
				}
			
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}

			private void traversePrefab(GameObject gameObject) {
				processComponents<Image>(gameObject);
				processComponents<Animator>(gameObject);
				processComponents<UIParticleSystem>(gameObject);
			}

			private void processComponents<T>(GameObject gameObject) where T : Component {
				T[] components = gameObject.GetComponentsInChildren<T>(true);
				foreach (var component in components) {
					if ((component as Image) is Image img) {
						if (needProcess(img.sprite)) {
							img.sprite = getNewAsset(img.sprite);
						}

						if (needProcess(img.material)) {
							img.material = getNewAsset(img.material);
						}
					}

					if ((component as Animator) is Animator animator) {
						if (needProcess(animator.runtimeAnimatorController)) {
							animator.runtimeAnimatorController = getNewAsset(animator.runtimeAnimatorController);
						}
					}

					if ((component as UIParticleSystem) is UIParticleSystem particleSystem) {
						if (needProcess(particleSystem.material)) {
							particleSystem.material = getNewAsset(particleSystem.material);
						}
					}
				}
			}

			/// <summary>
			///  Replaces references in prefab's Scripts if this reference is EXTERNAL prefab(Not in the same prefab)
			/// </summary>
			private void changeScripts() {
				GameObject[] prefabs = getAssetsAtPath<GameObject>();

				foreach (var prefab in prefabs) {
					checkScriptsInChild(prefab.transform);
				}
			}

			private void checkScriptsInChild(Transform parent, Object target = null) {
				void findScriptRefs(GameObject obj) {
					Component[] components = obj.GetComponents<Component>();
					for (int i = 0; i < components.Length; i++) {
						var c = components[i];
						if (!c) {
							continue;
						}

						var so = new SerializedObject(c);
						var sp = so.GetIterator();

						while (sp.NextVisible(true)) {
							if (sp.propertyType == SerializedPropertyType.ObjectReference) {
								if (sp.objectReferenceValue != target) {
									if (needProcess(sp.objectReferenceValue)) {
										var obj2 = getNewAsset(sp.objectReferenceValue);
										sp.objectReferenceValue = obj2;
									}
								}
							}
						}

						so.ApplyModifiedProperties();
						AssetDatabase.SaveAssets();
					}
				}

				findScriptRefs(parent.gameObject);
				int count = parent.childCount;
				for (int i = 0; i < count; i++) {
					Transform child = parent.GetChild(i);
					bool isRoot = PrefabUtility.IsOutermostPrefabInstanceRoot(child.gameObject);
					if (isRoot) {
						continue;
					}

					checkScriptsInChild(child);
				}
			}
		
			/// <summary>
			///  Replace nested prefabs in prefabs
			/// </summary>
			private void changeRegularInRegular() {
				GameObject[] prefabs = getAssetsAtPath<GameObject>().Where(o => !PrefabUtility.IsPartOfVariantPrefab(o)).ToArray();

				foreach (var prefab in prefabs) {
					getNestedPrefabs(prefab);
				}

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
			/// <summary>
			///  Replaces nested variant prefabs in prefabs
			/// </summary>
			private void changeVariantsInRegular() {
				GameObject[] prefabs = getAssetsAtPath<GameObject>().Where(o => !PrefabUtility.IsPartOfVariantPrefab(o)).ToArray();
			
				foreach (var prefab in prefabs) {
					getVariants(prefab);
				}

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}

			/// <summary>
			///  Replaces nested prefabs in prefab variants
			/// </summary>
			private void changeRegularInVariants() {
				GameObject[] prefabs = getAssetsAtPath<GameObject>().Where(PrefabUtility.IsPartOfVariantPrefab).ToArray();

				foreach (var prefab in prefabs) {
					getNestedPrefabs(prefab);
				}

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		
			/// <summary>
			///  Replaces nested variant prefabs in variants
			/// </summary>
			private void changeVariantsInVariants() {
				GameObject[] prefabs = getAssetsAtPath<GameObject>().Where(PrefabUtility.IsPartOfVariantPrefab).ToArray();

				foreach (var prefab in prefabs) {
					getVariants(prefab);
				}

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}

			private T[] getAssetsAtPath<T>() where T : Object {
				Object[] objectsInFolder = getAssetsAtPath<Object>(targetPath);
				return objectsInFolder.OfType<T>().ToArray();
			}

			private void getNestedPrefabs(GameObject root) {
				var instance = PrefabUtility.LoadPrefabContents(AssetDatabase.GetAssetPath(root));
				Func<GameObject, bool> checker = o => PrefabUtility.IsOutermostPrefabInstanceRoot(o) && !PrefabUtility.IsPartOfVariantPrefab(o);
				Func<Object, Object> sourcePrefabGetter = PrefabUtility.GetCorrespondingObjectFromOriginalSource;
				bool changed = false;
				checkPrefabsInRoot(instance.transform, checker, sourcePrefabGetter, ref changed);
				if (changed) {
					PrefabUtility.SaveAsPrefabAssetAndConnect(instance, AssetDatabase.GetAssetPath(root), InteractionMode.AutomatedAction);

				}
			}

			private void getVariants(GameObject root) {
				var instance = PrefabUtility.LoadPrefabContents(AssetDatabase.GetAssetPath(root));

				Func<GameObject, bool> checker = o => PrefabUtility.IsOutermostPrefabInstanceRoot(o) && PrefabUtility.IsPartOfVariantPrefab(o);
				Func<Object, Object> sourcePrefabGetter = PrefabUtility.GetCorrespondingObjectFromSource;
				bool changed = false;
				checkPrefabsInRoot(instance.transform, checker, sourcePrefabGetter, ref changed);
				if (changed) {
					PrefabUtility.SaveAsPrefabAssetAndConnect(instance, AssetDatabase.GetAssetPath(root), InteractionMode.AutomatedAction);
				}
			}

			private void checkPrefabsInRoot(Transform parent, Func<GameObject, bool> isRootChecker, Func<Object, Object> factory, ref bool changed) {
				int count = parent.childCount;
				for (int i = 0; i < count; i++) {
					var child = parent.transform.GetChild(i);
					bool isRoot = isRootChecker(child.gameObject);
					if (!isRoot) {
						checkPrefabsInRoot(child, isRootChecker, factory, ref changed);
					}
					else {
						var mainPrefab = factory(child.gameObject);
						if (!needProcess(mainPrefab)) {
							continue;
						}

						var newChild = getNewAsset(mainPrefab) as GameObject;
						newChild = (GameObject) PrefabUtility.InstantiatePrefab(newChild);
						if (newChild != null) {
							var pos = child.localPosition;
							var scale = child.localScale;
							var rot = child.localRotation;
							newChild.transform.SetParent(parent.transform);
							newChild.transform.SetSiblingIndex(i);
							newChild.transform.localPosition = pos;
							newChild.transform.localScale = scale;
							newChild.transform.rotation = rot;
							DestroyImmediate(child.gameObject, true);
							PrefabUtility.ApplyPrefabInstance(newChild, InteractionMode.AutomatedAction);
							changed = true;
						}
					}
				}
			}

			/// <summary>
			///  Traverse all directories in current path
			/// </summary>
			/// <param name="path">A Unity internal path with Assets/ prefix</param>
			/// <typeparam name="T">Any special type to retrieve </typeparam>
			/// <returns>An array of all objects in given path</returns>
			private T[] getAssetsAtPath<T>(string path) {
				var tempObjects = new ArrayList();

				string searchPath = Application.dataPath + "/" + path.Replace("Assets/", string.Empty);
				string[] fileEntries = Directory.GetFiles(searchPath);
				foreach (string fileName in fileEntries) {
					int index = fileName.LastIndexOf(Path.DirectorySeparatorChar);
					string localPath = path;

					if (index > 0) {
						localPath += fileName.Substring(index);
					}

					var t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

					if (t != null) {
						tempObjects.Add(t);
					}
				}

				string[] nestedFolders = Directory.GetDirectories(searchPath);
				if (nestedFolders.Length > 0) {
					var regex = new Regex(".*(?=Assets.*)");
					foreach (string entry in nestedFolders) {
						string folderUnityPath = regex.Replace(entry, string.Empty);
						tempObjects.Add(AssetImporter.GetAtPath(folderUnityPath));
						Object[] nestedFolderContent = getAssetsAtPath<Object>(folderUnityPath);
						tempObjects.AddRange(nestedFolderContent);
					}
				}

				T[] result = new T[tempObjects.Count];
				for (int i = 0; i < tempObjects.Count; i++) {
					result[i] = (T) tempObjects[i];
				}

				return result;
			}
		}
	}
}