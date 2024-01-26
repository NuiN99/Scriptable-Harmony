using System;
using NuiN.ScriptableHarmony.Core;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace NuiN.ScriptableHarmony.Sound
{
    [CreateAssetMenu(menuName = "ScriptableHarmony/Sound/New Sound Player", fileName = "New Sound Player")]
    public class SoundPlayerSO : ScriptableObject
    {
        ObjectPool<AudioSource> _sourcePool;
        Transform _pooledSourcesContainer;
        AudioSource _sourcePrefab;

        [SerializeField, Range(0,1)] float volume = 0.5f;

        [Header("Options")]
        [SerializeField] bool disableAudio;
        [SerializeField] AudioMixerGroup mixerGroup;
        [SerializeField] string prefsVolumeKey;

        string PrefsVolumeKey => "SH_" + prefsVolumeKey;
        public bool AudioDisabled => disableAudio;
        public float Volume => volume;
        
        void OnEnable()
        {
            if (prefsVolumeKey != string.Empty) volume = GetPrefsVolume();
            
            _sourcePrefab = Resources.Load<AudioSource>("SH_AudioSourcePrefab");

            SceneManager.activeSceneChanged += SetupForNewScene;
        }
        void OnDisable()
        {
            SceneManager.activeSceneChanged -= SetupForNewScene;
        }

        void OnValidate() => SetVolume(volume);
        
        public float GetPrefsVolume() => PlayerPrefs.HasKey(PrefsVolumeKey) ? PlayerPrefs.GetFloat(PrefsVolumeKey) : volume;

        public void SetVolume(float newVolume)
        {
            volume = Mathf.Clamp01(newVolume);
            PlayerPrefs.SetFloat(PrefsVolumeKey, volume);
            PlayerPrefs.Save();
        }
        
        void SetupForNewScene(Scene from, Scene to)
        {
            if (!this)
            {
                SceneManager.activeSceneChanged -= SetupForNewScene;
                return;
            }
            
            _pooledSourcesContainer = new GameObject(name + " | ObjectPool").transform;
            
            _sourcePool = new ObjectPool<AudioSource>(
                createFunc: () =>
                {
                    AudioSource source = Instantiate(_sourcePrefab, _pooledSourcesContainer);
                    source.name = "AudioSource_Pooled";
                    return source;
                },
                actionOnGet: s =>
                {
                    s.gameObject.SetActive(true);
                },
                actionOnRelease: s =>
                {
                    s.transform.SetParent(_pooledSourcesContainer);
                    s.gameObject.SetActive(false);
                });
        }

        // ReSharper disable Unity.PerformanceAnalysis 
        AudioSource InitializeNewSource(SoundSO soundObj, bool spatial, float volumeMult, float pitchMult)
        {
            AudioSource source = _sourcePool.Get();
            
            AudioClip clip = soundObj.Clip;
            if (clip == null) 
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning($"SoundSO {soundObj.name}: Attempted to play null clip", soundObj);
                #endif
                
                return source;
            }
            
            source.Stop();
            
            source.playOnAwake = false;
            source.clip = clip;
            source.outputAudioMixerGroup = mixerGroup;
            source.loop = soundObj.Loop;
            source.priority = soundObj.Priority;
            source.volume = soundObj.Volume * volume * volumeMult * MasterVolumeManager.GlobalVolume;
            source.pitch = soundObj.Pitch * pitchMult;
            source.panStereo = soundObj.StereoPan;
            source.reverbZoneMix = soundObj.ReverbZoneMix;
            source.spatialBlend = spatial ? 1f : 0f;
            
            source.Play();

            float lifetime = source.clip.length / Mathf.Max(Math.Abs(source.pitch), Mathf.Epsilon);
            SORuntimeHelper.DoAfter(lifetime, () => _sourcePool.Release(source));
            
            return source;
        }

        internal AudioSource Play(SoundSO sound, float volumeMult = 1f, float pitchMult = 1f)
        {
            return AudioDisabled ? new AudioSource() : InitializeNewSource(sound, false, volumeMult, pitchMult);
        }

        internal AudioSource PlaySpatial(SoundSO sound, Vector3 position, Transform parent = null, float volumeMult = 1f, float pitchMult = 1f)
        {
            if (AudioDisabled) return new AudioSource();

            AudioSource source = InitializeNewSource(sound, true, volumeMult, pitchMult);

            if (source.clip == null)
            {
                _sourcePool.Release(source);
                return source;
            }
            
            source.transform.position = position;
            
            if(parent != null) source.transform.SetParent(parent);

            return source;
        }
    }
}
