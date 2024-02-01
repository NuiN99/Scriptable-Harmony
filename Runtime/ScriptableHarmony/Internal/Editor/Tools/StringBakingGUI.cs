using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Editor
{
    internal class StringBakingGUI
    {
        const string TEMPLATE = 
@"
namespace NuiN.ScriptableHarmony
{
    public static class BakedStrings
    {
        public static class Tags
        {
            {TAGS}
        }

        public static class Layers
        {
            {LAYERS}
        }
    }
}";
        
        SelectionPathController _pathController;

        public StringBakingGUI(SelectionPathController pathController)
        {
            _pathController = pathController;
        }
        
        public void DrawGUI()
        {
            _pathController.DisplayPathGUI();
            
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                normal = { textColor = new Color(1, 0.3f, 0f, 1f) },
                fixedWidth = 75,
                fixedHeight = 25
            };
            Color ogColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.6f, 0.9f, 1f, 1f);
            if (GUILayout.Button("Create", buttonStyle))
            {
                GenerateScriptFile();
            }
            GUI.backgroundColor = ogColor;
        }
        
        void GenerateScriptFile()
        {
            const string filePathSOName = "SH_BakedStringsFilePath";

            FilePathAnchorSO pathAnchorSO = Resources.Load<FilePathAnchorSO>(filePathSOName);
            bool exists = pathAnchorSO != null;

            if (!exists)
            {
                pathAnchorSO = ScriptableObject.CreateInstance<FilePathAnchorSO>();

                string outerPath = _pathController.SelectionPath + "/BakedStrings";
                Directory.CreateDirectory(outerPath);
                
                string resourcesPath = outerPath + "/Resources";
                Directory.CreateDirectory(resourcesPath);

                string assetPath = $"{resourcesPath}/{filePathSOName}.asset";
                string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

                AssetDatabase.CreateAsset(pathAnchorSO, uniqueAssetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
            }


            string folderPath = pathAnchorSO.GetPath();
            Debug.Log(folderPath);
            string filePath = Path.Combine(folderPath, "BakedStrings.cs");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(TEMPLATE);
            }

            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }
    }
}