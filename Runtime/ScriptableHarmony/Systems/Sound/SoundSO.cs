using System;
using System.Collections.ObjectModel;
using System.Linq;
using NuiN.NExtensions;
using NuiN.NExtensions.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace NuiN.ScriptableHarmony.Sound
{
    public class SoundSO : ScriptableObject
    {
        [SerializeField] AudioMixerGroup audioMixerGroup;
        [SerializeField] AudioClip[] clips;
        
        [SerializeField, Range(0f, 1f), Tooltip("Default - 0.5")] float minVolume = 0.5f;
        [SerializeField, Range(0f, 1f), Tooltip("Default - 0.5")] float maxVolume = 0.5f;
        [SerializeField] bool useFixedVolumes;
        [SerializeField, HideInInspector, Range(0f, 1f)] float[] possibleVolumes;

        [SerializeField, Range(0.05f, 3f), Tooltip("Default - 1")] float minPitch = 1f;
        [SerializeField, Range(0.05f, 3f), Tooltip("Default - 1")] float maxPitch = 1f;
        [SerializeField] bool useFixedPitches;
        [SerializeField, HideInInspector, Range(0.05f, 3f)] float[] possiblePitches;

        [SerializeField, Range(-1f, 1f), Tooltip("Default - 0")] float stereoPan;
        [SerializeField, Range(0, 256), Tooltip("Default - 128")] int priority = 128;
        [SerializeField, Range(0f, 1.1f), Tooltip("Default - 1")] float reverbZoneMix = 1f;
        [SerializeField, Tooltip("Default - False")] bool loop;

        public AudioMixerGroup AudioMixerGroup => audioMixerGroup;
        public ReadOnlyCollection<AudioClip> Clips => Array.AsReadOnly(clips);
        public float MinVolume => minVolume;
        public float MaxVolume => maxVolume;
        public float MinPitch => minPitch;
        public float MaxPitch => maxPitch;
        public float StereoPan => stereoPan;
        public int Priority => priority;
        public float ReverbZoneMix => reverbZoneMix;
        public bool Loop => loop;
        public bool UseFixedVolumes => useFixedVolumes;
        public bool UseFixedPitches => useFixedPitches;
        public float Volume => useFixedVolumes && possibleVolumes.Length > 0 
            ? possibleVolumes[Random.Range(0, possibleVolumes.Length)] 
            : Random.Range(MinVolume, MaxVolume);
        public float Pitch => useFixedPitches && possiblePitches.Length > 0 
            ? possiblePitches[Random.Range(0, possiblePitches.Length)] 
            :Random.Range(MinPitch, MaxPitch);
        public AudioClip Clip => Clips[Random.Range(0, Clips.Count)];

        public AudioSource Play(float volumeMult = 1f, float pitchMult = 1f)
        {
            return !ClipsAreValid() ? new AudioSource() : SoundPlayer.Play(this, volumeMult, pitchMult);
        }

        public AudioSource PlaySpatial(Vector3 position, Transform parent = null, float volumeMult = 1f, float pitchMult = 1f)
        {
            return !ClipsAreValid() ? new AudioSource() : SoundPlayer.PlaySpatial(this, position, parent, volumeMult, pitchMult);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public bool ClipsAreValid()
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

        void OnValidate()
        {
            if (useFixedVolumes && possibleVolumes.Length == 0) possibleVolumes = new[] { 0.5f };
            if (useFixedPitches && possiblePitches.Length == 0) possiblePitches = new[] { 1f };
        }
    }
    
#if UNITY_EDITOR
    
    [CustomEditor(typeof(SoundSO))]
    public class SoundSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SoundSO soundSO = (SoundSO)target;
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("audioMixerGroup"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("clips"));
            
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

            EditorGUILayout.LabelField("Volume", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useFixedVolumes"));
            if (soundSO.UseFixedVolumes)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("possibleVolumes"));
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("minVolume"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("maxVolume"));
            }
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
            
            EditorGUILayout.LabelField("Pitch", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useFixedPitches"));
            if (soundSO.UseFixedPitches)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("possiblePitches"));
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("minPitch"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("maxPitch"));
            }
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

            EditorGUILayout.LabelField("Other", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stereoPan"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("priority"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("reverbZoneMix"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("loop"));

            serializedObject.ApplyModifiedProperties();
        }
    }
    
    /// <summary>
    /// Creates a SoundSO that contains any selected AudioClips
    /// </summary>
    internal static class CreateSoundSOContextMenu
    {
        const string NEW_SOUND_NAME = "New Sound";
    
        [MenuItem("Assets/Create/ScriptableHarmony/Sound/New Sound", false, 0)]
        static void CreateSoundSO()
        {
            AudioClip[] selectedClips = Selection.objects?
                .Where(obj => obj != null && obj is AudioClip)
                .Select(obj => (AudioClip)obj).ToArray();

            selectedClips ??= new AudioClip[] { };
            
            Object newSoundObj = SoundSO.CreateInstance(selectedClips.ToArray());

            string directory = SelectionPath.GetPath();
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
