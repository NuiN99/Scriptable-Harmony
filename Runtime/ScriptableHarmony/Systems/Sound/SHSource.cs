using System.Diagnostics.Tracing;
using NuiN.NExtensions;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Sound
{
    public class SHSource : MonoBehaviour
    {
        SoundSO _soundSO;

        [SerializeField, Range(0f, 1f)] float volumeScale = 0.5f;
        [SerializeField] AudioSource source;
        
        #if UNITY_EDITOR
        [SerializeField, ReadOnlyPlayMode, Range(0f, 1f)] float clipProgress;
        #endif

        public AudioSource AudioSource => source;
        public float VolumeScale { get => volumeScale; set => volumeScale = Mathf.Clamp01(value); }
        
        void Reset()
        {
            source = GetComponent<AudioSource>();
        }

        internal void AssignSound(SoundSO sound)
        {
            _soundSO = sound;
        }

        internal void UpdateVolume()
        {
            source.volume = volumeScale * _soundSO.SoundPlayer.Volume * MasterVolumeManager.GlobalVolume;
            
            #if UNITY_EDITOR
            clipProgress = source.time / source.clip.length;
            #endif
        }
    }
}

