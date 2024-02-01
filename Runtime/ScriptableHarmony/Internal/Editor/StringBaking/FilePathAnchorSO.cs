using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Editor
{
    [CreateAssetMenu(menuName = "ScriptableHarmony/File Path Anchor")]
    internal class FilePathAnchorSO : ScriptableObject
    {
        public string GetPath() => AssetDatabase.GetAssetPath(this).Replace($"/Resources/{name}.asset", "");
    }
}

