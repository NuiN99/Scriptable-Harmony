#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace NuiN.NExtensions
{
    internal static class StringBakerWindow
    {
        const string SCRIPT_TEMPLATE = 
@"using UnityEngine;
namespace NuiN.NExtensions
{
    public static class {SCRIPTNAME}
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

        const string SCRIPT_NAME = "Baked";
        const string FOLDER_NAME = "BakedStrings";
        
        const string NEW_LINE = "\n            ";
        const string LAYER_TEMPLATE = "public static readonly int {VARIABLENAME} = LayerMask.NameToLayer(\"{NAME}\");";
        const string TAG_TEMPLATE = "public static readonly string {VARIABLENAME} = \"{NAME}\";";
        const string SCENE_TEMPLATE = default;

        [MenuItem("Window/Bake Strings", priority = -100)]
        static void BakeStrings()
        {
            const string filePathSOName = "DO NOT MODIFY";

            PathAnchorSO pathAnchorSO = Resources.Load<PathAnchorSO>(filePathSOName);
            bool exists = pathAnchorSO != null;

            if (!exists)
            {
                pathAnchorSO = ScriptableObject.CreateInstance<PathAnchorSO>();

                string outerPath = SelectionPath.GetPath() + $"/{FOLDER_NAME}";
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
            string filePath = Path.Combine(folderPath, $"{SCRIPT_NAME}.cs");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(GetScriptContents());
            }

            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }

        static string GetScriptContents()
        {
            string layersContent = string.Empty;
            string tagsContent = string.Empty;
            foreach (string layer in GetAllLayers()) layersContent += ReplacedStringTemplate(LAYER_TEMPLATE, layer);
            foreach (string tag in GetAllTags()) tagsContent += ReplacedStringTemplate(TAG_TEMPLATE, tag);
            
            string contents = SCRIPT_TEMPLATE
                .Replace("{SCRIPTNAME}", SCRIPT_NAME)
                .Replace("{TAGS}", tagsContent)
                .Replace("{LAYERS}", layersContent);

            return contents;
        }

        static string ReplacedStringTemplate(string template, string name)
        {
            string variableName = Regex.Replace(name, @"\s+", "");
            string replaced = template
                .Replace("{NAME}", name)
                .Replace("{VARIABLENAME}", variableName)
                + NEW_LINE;

            return replaced;
        }

        static IEnumerable<string> GetAllLayers() => Enumerable.Range(0, 32).Select(LayerMask.LayerToName).Where(layer => !string.IsNullOrEmpty(layer)).ToArray();
        static IEnumerable<string> GetAllTags() => UnityEditorInternal.InternalEditorUtility.tags;
    }
}
#endif