#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NuiN.NExtensions.Editor
{
    internal static class StringBaker
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

        public static class Scenes
        {
            {SCENES}
        }
    }
}";
        const string SCRIPT_NAME = "Strings";
        const string ROOT_FOLDER = "Assets/Scripts/BakedStrings";

        const string NEW_LINE = "\n            ";
        const string LAYER_TEMPLATE = "public static readonly int {VARIABLENAME} = LayerMask.NameToLayer(\"{NAME}\");";
        const string TAG_TEMPLATE = "public static readonly string {VARIABLENAME} = \"{NAME}\";";
        const string SCENE_TEMPLATE = "public static readonly string {VARIABLENAME} = \"{NAME}\";";

        [MenuItem("Tools/SH/Bake Strings")]
        static void BakeStrings()
        {
            string defaultPath = Path.Combine(ROOT_FOLDER, $"{SCRIPT_NAME}.cs").Replace("\\", "/");
            string targetPath = defaultPath;

            string[] foundScripts = AssetDatabase.FindAssets($"{SCRIPT_NAME} t:Script");

            foreach (string guid in foundScripts)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid).Replace("\\", "/");
                if (IsInBakedStringsFolder(assetPath))
                {
                    targetPath = assetPath;
                    break;
                }
            }

            if (targetPath == defaultPath && !Directory.Exists(ROOT_FOLDER))
                Directory.CreateDirectory(ROOT_FOLDER);

            File.WriteAllText(targetPath, GetScriptContents());
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }

        static bool IsInBakedStringsFolder(string path)
        {
            path = path.Replace("\\", "/").ToLowerInvariant();
            return path.Contains("/bakedstrings/");
        }

        static string GetScriptContents()
        {
            string layersContent = string.Empty;
            string tagsContent = string.Empty;
            string scenesContent = string.Empty;

            layersContent = GetAllLayers().Aggregate(layersContent, (current, layer) => current + ReplacedStringTemplate(LAYER_TEMPLATE, layer));
            tagsContent = GetAllTags().Aggregate(tagsContent, (current, tag) => current + ReplacedStringTemplate(TAG_TEMPLATE, tag));
            scenesContent = GetAllScenes().Aggregate(scenesContent, (current, scene) => current + ReplacedStringTemplate(SCENE_TEMPLATE, scene));

            return SCRIPT_TEMPLATE
                .Replace("{SCRIPTNAME}", SCRIPT_NAME)
                .Replace("{TAGS}", tagsContent)
                .Replace("{LAYERS}", layersContent)
                .Replace("{SCENES}", scenesContent);
        }

        static string SafeVarName(string raw)
        {
            string cleaned = Regex.Replace(raw, "[^a-zA-Z0-9_]", "");
            if (string.IsNullOrEmpty(cleaned))
                cleaned = "_";
            if (char.IsDigit(cleaned[0]))
                cleaned = "_" + cleaned;
            return cleaned;
        }

        static string ReplacedStringTemplate(string template, string name)
        {
            string variableName = SafeVarName(name);
            return template
                .Replace("{NAME}", name)
                .Replace("{VARIABLENAME}", variableName)
                + NEW_LINE;
        }

        static IEnumerable<string> GetAllLayers() =>
            Enumerable.Range(0, 32)
                      .Select(LayerMask.LayerToName)
                      .Where(layer => !string.IsNullOrEmpty(layer));

        static IEnumerable<string> GetAllTags() =>
            UnityEditorInternal.InternalEditorUtility.tags;

        static IEnumerable<string> GetAllScenes()
        {
            var scenes = new List<string>();
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string sceneName = Path.GetFileNameWithoutExtension(
                    SceneUtility.GetScenePathByBuildIndex(i));
                scenes.Add(sceneName);
            }
            return scenes;
        }
    }
}
#endif
