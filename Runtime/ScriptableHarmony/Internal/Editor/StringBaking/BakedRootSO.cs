using NuiN.NExtensions;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Editor
{
    [CreateAssetMenu(menuName = "ScriptableHarmony/File Path Anchor")]
    internal class BakedRootSO : ScriptableObject
    {
        const string INFO = 
@"Do not alter the file structure of the folder

This ScriptableObject is used to find the Baked class when baking strings
";

        [SerializeField, ReadOnly, TextArea] string info = INFO;

        public string GetPath() => AssetDatabase.GetAssetPath(this).Replace($"/Resources/{name}.asset", "");
        
        string WarningRemoval() => info;
    }
}

