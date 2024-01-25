using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NuiN.ScriptableHarmony.Editor
{
#if UNITY_EDITOR
    internal class SelectionPathController
    {
        const string EMPTY_PATH_MESSAGE = "Please select an asset in the Project panel";

        EditorWindow _window;
        public string SelectionPath { get; private set; }
        public bool EmptyPath => string.IsNullOrEmpty(SelectionPath);
        
        public SelectionPathController(EditorWindow window)
        {
            _window = window;
            EditorApplication.update += Update;
        }
        public void Dispose()
        {
            EditorApplication.update -= Update;
        }

        public void DisplayPathGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            GUILayout.BeginHorizontal();
            
            GUILayout.Label("Path:", GUILayout.Width(50));
            EditorGUILayout.TextField(string.IsNullOrEmpty(SelectionPath) ? EMPTY_PATH_MESSAGE : SelectionPath, GUILayout.ExpandWidth(true));
            
            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }

        public void Update()
        {
            string path = "Assets";
            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path) || !File.Exists(path)) continue;
                
                path = Path.GetDirectoryName(path);
                break;
            }

            SelectionPath = path;
            
            if(_window != null) _window.Repaint();
        }
        
        static string GetSelectedFolderPath()
        {
            Object[] selectedObjects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
            if (selectedObjects.Length <= 0) return null;
            
            string path = AssetDatabase.GetAssetPath(selectedObjects[0]);
            if (File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }
            return path;
        }
    }
#endif
}