using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NuiN.NExtensions
{
#if UNITY_EDITOR
    public static class SelectionPath
    {
        public static string GetPath()
        {
            string path = "Assets";
            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path) || !File.Exists(path)) continue;
                
                path = System.IO.Path.GetDirectoryName(path);
                break;
            }

            return path;
        }
        
        public static void DrawGUI(EditorWindow window)
        {
            EditorGUI.BeginDisabledGroup(true);
            GUILayout.BeginHorizontal();
            
            GUILayout.Label("Path:", GUILayout.Width(50));
            string path = GetPath();
            EditorGUILayout.TextField(path, GUILayout.ExpandWidth(true));
            
            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
            
            window.Repaint();
        }
    }
#endif
}