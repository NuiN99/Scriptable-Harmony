using System;
using System.Collections.Generic;
using System.Linq;
using NuiN.NExtensions;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace NuiN.ScriptableHarmony.Sound
{
    [CreateAssetMenu(menuName = "ScriptableHarmony/Sound/New Sound Player", fileName = "New Sound Player")]
    public class SoundPlayerSO : ScriptableObject
    {
        UnityEngine.Pool.ObjectPool<AudioSource> _sourcePool;
            
        AudioSource _activeSource;
        bool _sceneDisabledAudio;
    
        [Range(0,1)] public float masterVolume = 0.5f;
        
        [SerializeField] AudioMixerGroup mixerGroup;
        
        [Header("Options")]
        public bool disableAudio;
        [SerializeField] List<string> disableAudioOnScenes;

        public bool AudioDisabled => disableAudio || _sceneDisabledAudio;
        
        void OnEnable() => SceneManager.activeSceneChanged += SetupForNewScene;
        void OnDisable() => SceneManager.activeSceneChanged -= SetupForNewScene;

        void SetupForNewScene(Scene from, Scene to)
        {
            if (disableAudioOnScenes.Any(sceneName => sceneName != null && sceneName == to.name))
            {
                _sceneDisabledAudio = true;
                return;
            }

            AudioSource sourcePrefab = new GameObject("AudioSourcePrefab").AddComponent<AudioSource>();
            _sourcePool = new ObjectPool<AudioSource>(
                createFunc: () => Instantiate(sourcePrefab),
                actionOnGet: s => s.gameObject.SetActive(true),
                actionOnRelease: s =>
                {
                    s.transform.SetParent(null);
                    s.gameObject.SetActive(false);
                });
            
            DontDestroyOnLoad(sourcePrefab);

            _sceneDisabledAudio = false;
        }

        AudioSource InitializeNewSource(SoundSettings settings, bool spatial)
        {
            AudioSource source = _sourcePool.Get();
            
            source.Stop();
            
            source.playOnAwake = false;
            source.clip = settings.Clip;
            source.outputAudioMixerGroup = mixerGroup;
            source.loop = settings.Loop;
            source.priority = settings.Priority;
            source.volume = settings.Volume * masterVolume;
            source.pitch = settings.Pitch;
            source.panStereo = settings.StereoPan;
            source.reverbZoneMix = settings.ReverbZoneMix;
            source.spatialBlend = spatial ? 1f : 0f;
            
            source.Play();

            float lifetime = source.clip.length / Mathf.Max(Math.Abs(source.pitch), Mathf.Epsilon);
            RuntimeHelper.DoAfter(lifetime, () => _sourcePool.Release(source));
            
            return source;
        }

        internal void Play(SoundSettings settings)
        {
            if (AudioDisabled) return;
            InitializeNewSource(settings, false);
        }

        internal void PlaySpatial(SoundSettings settings, Vector3 position, Transform parent = null)
        {
            if (AudioDisabled) return;

            AudioSource source = InitializeNewSource(settings, true);
            source.transform.position = position;
            source.transform.SetParent(parent);
        }
    }
}
