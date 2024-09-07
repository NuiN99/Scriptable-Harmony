using System.IO;
using UnityEditor;
using Object = UnityEngine.Object;

namespace NuiN.NExtensions.Editor
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
    }
#endif
}