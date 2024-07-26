using System.Collections.Generic;
using System.IO;
using NuiN.NExtensions;
using NuiN.ScriptableHarmony.Core;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony
{
    public class AssemblyContainerSO : ScriptableObject
    {
        static AssemblyContainerSO instance;
        public static AssemblyContainerSO Instance => GetAssemblyContainer();

        static AssemblyContainerSO GetAssemblyContainer()
        {
            if (instance != null) return instance;
            instance = Resources.Load<AssemblyContainerSO>("SH_AssemblyContainer");
            if (instance != null) return instance;
            
#if UNITY_EDITOR
            AssetDatabase.CreateFolder("Assets", "Resources");
            AssemblyContainerSO asset = CreateInstance<AssemblyContainerSO>();
            AssetDatabase.CreateAsset(asset, "Assets/Resources/SH_AssemblyContainer.asset");
            AssetDatabase.SaveAssets();
            instance = asset;
#endif
            return instance;
        }

        [field: SerializeField, ReadOnly] public List<string> RegisteredAssemblies { get; set; } = new();

        #if UNITY_EDITOR
        void OnEnable() => OnSaveEvent.OnSave += FindAndRegister;
        void OnDisable() => OnSaveEvent.OnSave -= FindAndRegister;
        void OnValidate() => FindAndRegister();
        #endif

        public void FindAndRegister()
        {
            #if UNITY_EDITOR
            if (this == null) return;
            
            List<string> registeredAssemblies = RegisteredAssemblies;
            if (registeredAssemblies == null) return;
            registeredAssemblies.Clear();
                
            // Assembly-CSharp doesn't exist when no scripts are using it
            const string assemblyCSharpPath = "Library/ScriptAssemblies/Assembly-CSharp.dll";
            if (File.Exists(assemblyCSharpPath))
            {
                registeredAssemblies.Add("Assembly-CSharp");
            }
                
            string[] guids = AssetDatabase.FindAssets("t:asmdef", new[] { "Assets" });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                    
                // editor assemblies don't exist in the build and throw errors
                if (path.Contains("/Editor/"))
                {
                    continue;
                }
                    
                string assetName = Path.GetFileNameWithoutExtension(path);
                registeredAssemblies.Add(assetName);
            }
            
            EditorUtility.SetDirty(this);
            #endif
        }
    }
}