using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NuiN.ScriptableHarmony.Sound
{
    [Serializable]
    internal class SoundSettings
    {
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
        [SerializeField, Range(-1f, 1f), Tooltip("Default - 0")] float stereoPan = 0f;
        [SerializeField, Range(0, 256), Tooltip("Default - 128")] int priority = 128;
        [SerializeField, Range(0f, 1.1f), Tooltip("Default - 1")] float reverbZoneMix = 1f;
        [SerializeField, Tooltip("Default - False")] bool loop = false;

        public AudioClip[] Clips => clips;
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
        public AudioClip Clip => Clips[Random.Range(0, Clips.Length)];

        internal void SetClips(AudioClip[] newClips) => clips = newClips;
    }
}