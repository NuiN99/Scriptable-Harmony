#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Sound
{
    public static class CreateSoundSOContextMenu
    {
        const string NEW_SOUND_NAME = "New SoundSO";
    
        [MenuItem("Assets/Create/ScriptableHarmony/Sound/Sound Object from Selection", false, 0)]
        public static void Test()
        {
            AudioClip[] selectedClips = Selection.objects?
                .Where(obj => obj != null && obj is AudioClip)
                .Select(obj => (AudioClip)obj).ToArray();
        
            if (selectedClips is { Length: <= 0 } or null)
            {
                Debug.LogWarning("No AudioClips Selected!");
                return;
            }
            
            SoundPlayerSO defaultSoundPlayer = Resources.Load<SoundPlayerSO>("Default Sound Player");

            Object newSoundObj = SoundSO.CreateInstance(selectedClips.ToArray(), defaultSoundPlayer);
        
            string directory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(selectedClips[0]));
            string assetPath = $"{directory}/{NEW_SOUND_NAME}.asset";
            string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
        
            AssetDatabase.CreateAsset(newSoundObj, uniqueAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newSoundObj;
        }
    }
}

#endif
