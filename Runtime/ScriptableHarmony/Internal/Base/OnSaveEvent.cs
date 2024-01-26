#if UNITY_EDITOR

using System;
using UnityEditor;

namespace NuiN.ScriptableHarmony.Core
{
    internal class OnSaveEvent : AssetModificationProcessor
    {
        public static event Action OnSave;

        static string[] OnWillSaveAssets(string[] paths)
        {
            OnSave?.Invoke();
            return paths;
        }
    }
}

#endif