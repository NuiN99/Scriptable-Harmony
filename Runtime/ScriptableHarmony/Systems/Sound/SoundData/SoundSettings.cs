using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NuiN.ScriptableHarmony.Sound
{
    [Serializable]
    internal class SoundSettings
    {
        [field: SerializeField] public AudioClip[] Clips { get; private set; }

        [field: SerializeField, Tooltip("Default - False")] public bool Loop { get; private set; }
        
        [field: SerializeField, Range(0, 256), Tooltip("Default - 128")] public int Priority { get; private set; } = 128;
        
        [field: Header("Volume"), SerializeField, Range(0f, 1f), Tooltip("Default - 0.5")] public float MinVolume { get; private set; } = 0.5f;
        [field: SerializeField, Range(0f, 1f), Tooltip("Default - 0.5")] public float MaxVolume { get; private set; } = 0.5f;
        
        [field: Header("Pitch"), SerializeField, Range(-3f, 3f), Tooltip("Default - 1")] public float MinPitch { get; private set; } = 1;
        [field: SerializeField, Range(-3f, 3f), Tooltip("Default - 1")] public float MaxPitch { get; private set; } = 1;
        
        [field: SerializeField, Range(-1f, 1f), Tooltip("Default - 0")] public float StereoPan { get; private set; }
        
        [field: SerializeField, Range(0f, 1.1f), Tooltip("Default - 1")] public float ReverbZoneMix { get; private set; } = 1f;

        public float Volume => Random.Range(MinVolume, MaxVolume);
        public float Pitch => Random.Range(MinPitch, MaxPitch);

        public AudioClip Clip => Clips[Random.Range(0, Clips.Length)];

        internal void SetClips(AudioClip[] newClips) => Clips = newClips;
    }
}