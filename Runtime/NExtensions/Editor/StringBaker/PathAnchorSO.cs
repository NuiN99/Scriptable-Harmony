#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
namespace NuiN.NExtensions
{
    internal class PathAnchorSO : ScriptableObject
    {
        const string INFO = 
@"Do not alter the file structure of the folder

This ScriptableObject is used to find Baked.cs when baking strings
";

        [SerializeField, ReadOnly, TextArea] string info = INFO;

        public string GetPath() => AssetDatabase.GetAssetPath(this).Replace($"/Resources/{name}.asset", "");
        
        string WarningRemoval() => info;
    }
}
#endif
