using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace NuiN.ScriptableHarmony.Sound
{
    public class SoundSO : ScriptableObject
    {
        [SerializeField] SoundPlayerSO soundPlayer;
        
        [SerializeField] AudioClip[] clips;
        
        [Header("Volume")]
        [SerializeField, Range(0f, 1f), Tooltip("Default - 0.5")] float minVolume = 0.5f;
        [SerializeField, Range(0f, 1f), Tooltip("Default - 0.5")] float maxVolume = 0.5f;
        [SerializeField] bool useSetVolumes;
        [SerializeField, Range(0f, 1f)] float[] possibleVolumes;

        [Header("Pitch")]
        [SerializeField, Range(0.05f, 3f), Tooltip("Default - 1")] float minPitch = 1f;
        [SerializeField, Range(0.05f, 3f), Tooltip("Default - 1")] float maxPitch = 1f;
        [SerializeField] bool useSetPitches;
        [SerializeField, Range(0.05f, 3f)] float[] possiblePitches;

        [Header("Other")]
        [SerializeField, Range(-1f, 1f), Tooltip("Default - 0")] float stereoPan;
        [SerializeField, Range(0, 256), Tooltip("Default - 128")] int priority = 128;
        [SerializeField, Range(0f, 1.1f), Tooltip("Default - 1")] float reverbZoneMix = 1f;
        [SerializeField, Tooltip("Default - False")] bool loop;

        public ReadOnlyCollection<AudioClip> Clips => Array.AsReadOnly(clips);
        public float MinVolume => minVolume;
        public float MaxVolume => maxVolume;
        public float MinPitch => minPitch;
        public float MaxPitch => maxPitch;
        public float StereoPan => stereoPan;
        public int Priority => priority;
        public float ReverbZoneMix => reverbZoneMix;
        public bool Loop => loop;
        public float Volume => useSetVolumes && possibleVolumes.Length > 0 
            ? possibleVolumes[Random.Range(0, possibleVolumes.Length)] 
            : Random.Range(MinVolume, MaxVolume);
        public float Pitch => useSetPitches && possiblePitches.Length > 0 
            ? possiblePitches[Random.Range(0, possiblePitches.Length)] 
            :Random.Range(MinPitch, MaxPitch);
        public AudioClip Clip => Clips[Random.Range(0, Clips.Count)];

        void Reset() => soundPlayer = Resources.Load<SoundPlayerSO>("Default Sound Player");
        
        public AudioSource Play(float volumeMult = 1f, float pitchMult = 1f)
        {
            return !ClipsAreValid() ? new AudioSource() : soundPlayer.Play(this, volumeMult, pitchMult);
        }

        public AudioSource PlaySpatial(Vector3 position, Transform parent = null, float volumeMult = 1f, float pitchMult = 1f)
        {
            return !ClipsAreValid() ? new AudioSource() : soundPlayer.PlaySpatial(this, position, parent, volumeMult, pitchMult);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        bool ClipsAreValid()
        {
            if (clips.Length != 0) return true;
            
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning($"SoundSO {name}: No audio clips in array", this);
            #endif
            
            return false;
        }
        
        public static SoundSO CreateInstance(AudioClip[] clips)
        {
            var obj = ScriptableObject.CreateInstance<SoundSO>();
            obj.clips = clips;
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