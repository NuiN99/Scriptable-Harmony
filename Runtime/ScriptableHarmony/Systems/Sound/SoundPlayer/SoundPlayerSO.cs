using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace NuiN.ScriptableHarmony.Sound
{
    [CreateAssetMenu(menuName = "ScriptableHarmony/Sound/Player", fileName = "New Sound Player")]
    public class SoundPlayerSO : ScriptableObject
    {
        AudioSource _activeSource;
        bool _sceneDisabledAudio;
    
        [Range(0,1)] public float masterVolume = 1f;
        
        [SerializeField] [Tooltip("Sets the Source's mixer group")] AudioMixerGroup mixerGroup;
        
        [Header("Prefabs")]
        [SerializeField] AudioSource source;
        [SerializeField] AudioSource spatialSource;

        [Header("Options")]
        public bool disableAudio;
        [SerializeField] List<string> disableAudioOnScenes;
        
        void OnEnable() => SceneManager.activeSceneChanged += CheckDisabledScenes;
        void OnDisable() => SceneManager.activeSceneChanged -= CheckDisabledScenes;

        void CheckDisabledScenes(Scene from, Scene to)
        {
            if (disableAudioOnScenes.Any(sceneName => sceneName != null && sceneName == to.name))
            {
                _sceneDisabledAudio = true;
                return;
            }

            _sceneDisabledAudio = false;
        }

        public void Play(SoundSO sound, float volumeMult = 1) 
            => Play(sound.Clip, sound.Volume * volumeMult);
        public void PlaySpatial(SoundSO sound, Vector3 position, float volumeMult = 1, Transform parent = null)
            => PlaySpatial(sound.Clip, position, sound.Volume * volumeMult, parent);
        
        public void PlayRandom(SoundArraySO soundArray, float volumeMult = 1)
            => PlayRandom(soundArray.Clips, soundArray.Volume * volumeMult);
        public void PlayRandomSpatial(SoundArraySO soundArray, Vector3 position, float volumeMult = 1, Transform parent = null)
            => PlayRandomSpatial(soundArray.Clips, position, soundArray.Volume * volumeMult, parent);
        
        public void PlayIndex(SoundArraySO soundArray, int index, float volumeMult = 1)
            => Play(soundArray.Clips[index], soundArray.Volume * volumeMult);
        public void PlayIndexSpatial(SoundArraySO soundArray, int index, Vector3 position, float volumeMult = 1, Transform parent = null)
            => PlaySpatial(soundArray.Clips[index], position, soundArray.Volume * volumeMult, parent);
        

        public void PlayAll(SoundArraySO soundArray, float volumeMult = 1f)
        {
            foreach (var clip in soundArray.Clips) Play(clip, soundArray.Volume * volumeMult);
        }
        public void PlayAllSpatial(SoundArraySO soundArray, Vector3 position, float volumeMult = 1f, Transform parent = null)
        {
            foreach (var clip in soundArray.Clips) PlaySpatial(clip, position, soundArray.Volume * volumeMult, parent);
        }

        void Play(AudioClip clip, float volume = 1)
        {
            if (disableAudio || _sceneDisabledAudio) return;
            
            if (_activeSource == null)
            {
                _activeSource = Instantiate(source);
                _activeSource.name = "Scriptable Harmony AudioSource";
                _activeSource.outputAudioMixerGroup = mixerGroup;
            }
        
            _activeSource.PlayOneShot(clip, volume * masterVolume);
        }
        void PlayRandom(IReadOnlyList<AudioClip> clips, float volume = 1)
        {
            if (disableAudio || _sceneDisabledAudio) return;
            
            AudioClip randClip = clips[Random.Range(0, clips.Count)];
            Play(randClip, volume);
        }
        
        void PlaySpatial(AudioClip clip, Vector3 position, float volume = 1, Transform parent = null)
        {
            if (disableAudio || _sceneDisabledAudio) return;
            
            AudioSource audioSource = Instantiate(spatialSource, position, Quaternion.identity, parent);
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.clip = clip;
            audioSource.volume = volume * masterVolume;
            audioSource.Play();
            Destroy(audioSource.gameObject, clip.length / Mathf.Max(Math.Abs(audioSource.pitch), Mathf.Epsilon));
        }

        void PlayRandomSpatial(IReadOnlyList<AudioClip> clips, Vector3 position, float volume = 1, Transform parent = null)
        {
            PlaySpatial(clips[Random.Range(0, clips.Count)], position, volume, parent);
        }
    }
}
