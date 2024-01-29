using System;
using NuiN.NExtensions;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace NuiN.ScriptableHarmony.Sound
{
    [CreateAssetMenu(menuName = "ScriptableHarmony/Sound/New Sound Player", fileName = "New Sound Player")]
    public class SoundPlayerSO : ScriptableObject
    {
        UnityEngine.Pool.ObjectPool<SHSource> _sourcePool;
        Transform _pooledSourcesContainer;
        SHSource _sourcePrefab;

        [SerializeField, Range(0,1)] float volume = 0.5f;

        [Header("Options")]
        [SerializeField] AudioMixerGroup mixerGroup;
        [SerializeField] string prefsVolumeKey;

        string PrefsVolumeKey => "SH_" + prefsVolumeKey;
        public float Volume => volume;
        
        void OnEnable()
        {
            if (prefsVolumeKey != string.Empty) volume = GetPrefsVolume();
            
            _sourcePrefab = Resources.Load<SHSource>("SH_AudioSourcePrefab");

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
            
            _sourcePool = new UnityEngine.Pool.ObjectPool<SHSource>(
                createFunc: () =>
                {
                    SHSource source = Instantiate(_sourcePrefab, _pooledSourcesContainer);
                    source.name = "SHSource_Pooled";
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
        SHSource InitializeNewSource(SoundSO soundObj, bool spatial, float volumeMult, float pitchMult)
        {
            SHSource shSource = _sourcePool.Get();
            
            AudioClip clip = soundObj.Clip;
            if (clip == null) 
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning($"SoundSO {soundObj.name}: Attempted to play null clip", soundObj);
                #endif
                
                return shSource;
            }

            shSource.AssignSound(soundObj);
            AudioSource source = shSource.AudioSource;
            
            source.Stop();
            
            source.playOnAwake = false;
            source.clip = clip;
            source.outputAudioMixerGroup = mixerGroup;
            source.loop = soundObj.Loop;
            source.priority = soundObj.Priority;
            source.pitch = soundObj.Pitch * pitchMult;
            source.panStereo = soundObj.StereoPan;
            source.reverbZoneMix = soundObj.ReverbZoneMix;
            source.spatialBlend = spatial ? 1f : 0f;

            shSource.VolumeScale = soundObj.Volume;
            
            source.Play();

            if (source.loop) return shSource;
            
            float lifetime = source.clip.length / Mathf.Max(Math.Abs(source.pitch), Mathf.Epsilon);
            RuntimeHelper.DoAfter(lifetime, () => { if(source != null) _sourcePool.Release(shSource); });

            return shSource;
        }

        internal SHSource Play(SoundSO sound, float volumeMult = 1f, float pitchMult = 1f)
        {
            return InitializeNewSource(sound, false, volumeMult, pitchMult);
        }

        internal SHSource PlaySpatial(SoundSO sound, Vector3 position, Transform parent = null, float volumeMult = 1f, float pitchMult = 1f)
        {
            SHSource source = InitializeNewSource(sound, true, volumeMult, pitchMult);

            if (source.AudioSource.clip == null)
            {
                _sourcePool.Release(source);
                return source;
            }
            
            source.transform.position = position;
            source.transform.SetParent(parent);

            return source;
        }

        internal void ReleaseToPool(SHSource source)
        {
            _sourcePool.Release(source);
        }
    }
}
