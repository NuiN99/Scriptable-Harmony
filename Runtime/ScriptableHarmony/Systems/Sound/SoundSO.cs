using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Sound
{
    public class SoundSO : ScriptableObject
    {
        [SerializeField] SoundPlayerSO player;
        [SerializeField] SoundSettings settings;

        public SoundSettings Settings => settings;
        
        void Reset() => player = Resources.Load<SoundPlayerSO>("Default Sound Player");

        // ReSharper disable Unity.PerformanceAnalysis 
        public void Play()
        {
            if (!ClipsAreValid()) return;
            player.Play(this);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void PlaySpatial(Vector3 position, Transform parent = null)
        {
            if (!ClipsAreValid()) return;
            player.PlaySpatial(this, position, parent);
        }

        bool ClipsAreValid()
        {
            if (settings.Clips.Count != 0) return true;
            
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning($"SoundSO {name}: No audio clips in array", this);
            #endif
            
            return false;
        }
        
        public static SoundSO CreateInstance(AudioClip[] clips)
        {
            var obj = ScriptableObject.CreateInstance<SoundSO>();
            obj.settings = new SoundSettings();
            obj.settings.SetClips(clips);
            return obj;
        }
    }
    
#if UNITY_EDITOR
    internal static class CreateSoundSOContextMenu
    {
        const string NEW_SOUND_NAME = "New Sound";
    
        [MenuItem("Assets/Create/ScriptableHarmony/Sound/New Sound", false, 0)]
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
            
            Object newSoundObj = SoundSO.CreateInstance(selectedClips.ToArray());
        
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
#endif
}