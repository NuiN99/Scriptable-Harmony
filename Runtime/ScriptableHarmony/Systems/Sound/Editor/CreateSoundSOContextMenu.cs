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
        const string NEW_SOUND_ARRAY_NAME = "New SoundArraySO";
    
        [MenuItem("Assets/Create/ScriptableHarmony/Sound/Sound Object from Selection", false, 0)]
        public static void Test()
        {
            List<AudioClip> selectedClips = Selection.objects?
                .Where(obj => obj != null && obj is AudioClip)
                .Select(obj => (AudioClip)obj).ToList();
        
            if (selectedClips is { Count: <= 0 })
            {
                Debug.LogWarning("No AudioClip Selected!");
                return;
            }

            bool isArray = selectedClips!.Count > 1;

            AudioClip clip = selectedClips[0];
            SoundPlayerSO defaultSoundPlayer = Resources.Load<SoundPlayerSO>("Default Sound Player");
            
            Object newSoundObj = isArray 
                ? SoundArraySO.CreateInstance(selectedClips.ToArray(), defaultSoundPlayer) 
                : SoundSO.CreateInstance(clip, defaultSoundPlayer);
            
            string assetName = isArray 
                ? NEW_SOUND_ARRAY_NAME 
                : NEW_SOUND_NAME;
        
            string directory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(clip));
            string assetPath = $"{directory}/{assetName}.asset";
            string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
        
            AssetDatabase.CreateAsset(newSoundObj, uniqueAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newSoundObj;
        }
    }
}


