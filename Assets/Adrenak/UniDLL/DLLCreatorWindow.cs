using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Adrenak.UniDLL {
	public class DLLCreatorWindow : EditorWindow {
		public List<string> defines = new List<string>();
		public List<string> references = new List<string>();
		public List<Object> codes = new List<Object>();
		public string dllName = string.Empty;
		Vector2 scrollPosition = new Vector2(0, 0);

		[MenuItem("Tools/UniDLL/DLL Creator")]
		static void Open() {
			var window = GetWindowWithRect<DLLCreatorWindow>(new Rect(0, 0, 500, 300));
			window.Show();
			window.minSize = new Vector2(200, 200);
			window.maxSize = new Vector2(700, 700);
			window.titleContent = new GUIContent("UniDLL");
		}

		void OnGUI() {
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			EditorGUILayout.BeginVertical();
			{
				DrawDLLName();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				DrawCode();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				DrawDefines();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				DrawReferences();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				if (GUILayout.Button("Create"))
					Create();

			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndScrollView();
		}

		void DrawDLLName() {
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("DLL Name");
				dllName = EditorGUILayout.TextField(dllName);
			}
			EditorGUILayout.EndHorizontal();
		}

		void DrawDefines() {
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Defines");
				
				if (GUILayout.Button("Add")) 
					defines.Add(string.Empty);

				EditorGUILayout.BeginVertical();
				{
					for (int i = 0; i < defines.Count; i++) {
						var index = i;
						EditorGUILayout.BeginHorizontal();
						{
							defines[i] = EditorGUILayout.TextField(defines[i]);
							if (GUILayout.Button("Remove")) {
								Debug.Log(index);
								defines.RemoveAt(index);
							}
						}
						EditorGUILayout.EndHorizontal();
					}
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();
		}

		void DrawReferences() {
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("References");

				if (GUILayout.Button("Add")) {
					Debug.Log(EditorApplication.applicationPath);
					string path = EditorUtility.OpenFilePanel("Select Reference", GetUnityDLLPath(), "*");
					if(!string.IsNullOrEmpty(path))
						references.Add(path);
				}

				EditorGUILayout.BeginVertical();
				{
					for (int i = 0; i < references.Count; i++) {
						var reference = references[i];
						EditorGUILayout.BeginHorizontal();
						{
							reference = EditorGUILayout.TextField(reference);
							if (GUILayout.Button("Remove"))
								references.Remove(reference);
						}
						EditorGUILayout.EndHorizontal();
					}
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();
		}

		void DrawCode() {
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Source");

				if (GUILayout.Button("Add"))
					codes.Add(new Object());

				EditorGUILayout.BeginVertical();
				{
					for (int i = 0; i < codes.Count; i++) {
						var index = i;
						EditorGUILayout.BeginHorizontal();
						{
							codes[i] = EditorGUILayout.ObjectField(codes[i], typeof(Object), false);
							if (GUILayout.Button("Remove"))
								codes.RemoveAt(index);
						}
						EditorGUILayout.EndHorizontal();
					}

				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();
		}

		void Create() {
			SetupDirectory();

			CSC csc = new CSC() {
				defines = defines
				.Where(define => !string.IsNullOrEmpty(define))
				.ToArray(),

				references = references
				.Where(reference => !string.IsNullOrEmpty(reference))
				.ToArray(),

				recurse = codes
				.Where(c => c != null)
				.Select(folder => GetAssetAbsPath(folder))
				.ToArray(),

				output = GetOutputFile()
			};
			csc.Build();
			AssetDatabase.Refresh();
		}

		void SetupDirectory() {
			Directory.CreateDirectory(GetOutputDirectory());

			try { File.Copy(GetProjectPath() + "LICENSE", GetOutputDirectory() + "LICENSE"); } catch { }
			try { File.Copy(GetProjectPath() + "LICENSE.md", GetOutputDirectory() + "LICENSE.md"); } catch { }
			try { File.Copy(GetProjectPath() + "LICENSE.txt", GetOutputDirectory() + "LICENSE.txt"); } catch { }

			try { File.Copy(GetProjectPath() + "README", GetOutputDirectory() + "README"); } catch { }
			try { File.Copy(GetProjectPath() + "README.md", GetOutputDirectory() + "README.md"); } catch { }
			try { File.Copy(GetProjectPath() + "README.txt", GetOutputDirectory() + "README.txt"); } catch { }
		}

		string GetProjectPath() {
			return Application.dataPath.Replace("Assets", "");
		}

		string GetAssetAbsPath(Object folder) {
			var relPath = AssetDatabase.GetAssetPath(folder.GetInstanceID());
			return Application.dataPath + relPath.Replace("Assets", "");
		}

		string GetOutputDirectory() {
			return Application.dataPath + "\\" + dllName + "-build\\";
		}

		string GetOutputFile() {
			return GetOutputDirectory() + "\\" + dllName + "-lib" + ".dll";
		}
		
		string GetUnityDLLPath() {
			return EditorApplication.applicationPath.Replace("Unity.exe", "") + @"\Data\Managed";
		}
	}
}
