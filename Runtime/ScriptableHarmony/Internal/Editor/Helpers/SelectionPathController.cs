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
            Selection.selectionChanged += UpdatePath;
            _window = window;
            SelectionPath = GetSelectedFolderPath();
        }
        public void Dispose()
        {
            Selection.selectionChanged -= UpdatePath;
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
        
        void UpdatePath()
        {
            string selectedPath = GetSelectedFolderPath();
            if (string.IsNullOrEmpty(selectedPath)) return;
            
            SelectionPath = selectedPath;
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