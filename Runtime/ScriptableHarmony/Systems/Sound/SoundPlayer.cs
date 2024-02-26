using System;
using NuiN.NExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NuiN.ScriptableHarmony.Sound
{
    public static class SoundPlayer
    {
        static UnityEngine.Pool.ObjectPool<AudioSource> sourcePool;
        static Transform pooledSourcesContainer;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void NullVariables()
        {
            sourcePool?.Dispose();
            sourcePool = null;
            pooledSourcesContainer = null;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis 
        static AudioSource InitializeNewSource(SoundSO soundObj, bool spatial, float volumeMult, float pitchMult)
        {
            if (sourcePool == null)
            {
                pooledSourcesContainer ??= new GameObject("AudioSource | ObjectPool").transform;
                sourcePool = new UnityEngine.Pool.ObjectPool<AudioSource>(
                    createFunc: () =>
                    {
                        AudioSource source = new GameObject().AddComponent<AudioSource>();
                        source.transform.SetParent(pooledSourcesContainer);
                        source.name = "AudioSource_Pooled";
                        return source;
                    },
                    actionOnGet: s =>
                    {
                        s.gameObject.SetActive(true);
                    },
                    actionOnRelease: s =>
                    {
                        s.transform.SetParent(pooledSourcesContainer);
                        s.gameObject.SetActive(false);
                    });
            }
            
            AudioSource source = sourcePool.Get();
            
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
            source.outputAudioMixerGroup = soundObj.AudioMixerGroup;
            source.loop = soundObj.Loop;
            source.priority = soundObj.Priority;
            source.pitch = soundObj.Pitch * pitchMult;
            source.panStereo = soundObj.StereoPan;
            source.reverbZoneMix = soundObj.ReverbZoneMix;
            source.spatialBlend = spatial ? 1f : 0f;

            source.volume = soundObj.Volume;
            source.Play();

            if (source.loop) return source;
            
            float lifetime = source.clip.length / Mathf.Max(Math.Abs(source.pitch), Mathf.Epsilon);
            RuntimeHelper.DoAfter(lifetime, () => { if(source != null) sourcePool.Release(source); });

            return source;
        }

        internal static AudioSource Play(SoundSO sound, float volumeMult = 1f, float pitchMult = 1f)
        {
            return InitializeNewSource(sound, false, volumeMult, pitchMult);
        }

        internal static AudioSource PlaySpatial(SoundSO sound, Vector3 position, Transform parent = null, float volumeMult = 1f, float pitchMult = 1f)
        {
            AudioSource source = InitializeNewSource(sound, true, volumeMult, pitchMult);

            if (source.clip == null)
            {
                sourcePool.Release(source);
                return source;
            }
            
            source.transform.position = position;
            source.transform.SetParent(parent);

            return source;
        }

        internal static void ReleaseToPool(AudioSource source)
        {
            sourcePool.Release(source);
        }
    }
}
