using System.Collections;
using System.Collections.Generic;
using NuiN.ScriptableHarmony.Core;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace NuiN.ScriptableHarmony.Particles
{
    public static class ParticleSpawner
    {
        static Dictionary<ParticleSystem, (ObjectPool<ParticleSystem> pool, Transform container)> prefabObjectPools = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void ResetObjectPoolDictionary()
        {
            prefabObjectPools = new Dictionary<ParticleSystem, (ObjectPool<ParticleSystem> pool, Transform container)>();

            SceneManager.sceneLoaded -= ResetObjectPoolDictionaryOnSceneLoad;
            SceneManager.sceneLoaded += ResetObjectPoolDictionaryOnSceneLoad;
        }

        static void ResetObjectPoolDictionaryOnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            ResetObjectPoolDictionary();
        }

        static (ObjectPool<ParticleSystem> pool, Transform container) CreatePool(ParticleSystem prefab)
        {
            Transform container = new GameObject($"{prefab.name} | ObjectPool").transform;
            
            var pool = new ObjectPool<ParticleSystem>(
                createFunc: () => Object.Instantiate(prefab, container),
                actionOnGet: system =>
                {
                    system.gameObject.SetActive(true);
                    system.Play();
                },
                actionOnRelease: system =>
                {
                    system.transform.SetParent(container);
                    system.gameObject.SetActive(false);
                    system.Stop();
                });

            return (pool, container);
        }
        
        public static ParticleSystem Spawn(ParticleSystem prefab, Vector3 position, Quaternion? rotation = null, Transform parent = null, float scaleMultiplier = 1f, float? lifetime = null)
        {
            Quaternion rot = rotation ?? prefab.transform.rotation;

            if (!prefabObjectPools.TryGetValue(prefab, out (ObjectPool<ParticleSystem> pool, Transform container) poolTuple))
            {
                poolTuple = CreatePool(prefab);
                prefabObjectPools.Add(prefab, poolTuple);
            }
            
            ParticleSystem particleSystem = poolTuple.pool.Get();
            
            particleSystem.transform.SetPositionAndRotation(position, rot);
            particleSystem.transform.localScale = prefab.transform.localScale * scaleMultiplier;
            particleSystem.transform.SetParent(parent);
            
            if (lifetime == null)
            {
                ParticleSystem.MainModule main = particleSystem.main;
                ParticleSystem.MinMaxCurve curve = main.startLifetime;
                float startLifetime;
                if (curve.curveMax is { length: > 0 })
                {
                    Keyframe lastFrame = curve.curveMax[curve.curveMax.length-1];
                    startLifetime = lastFrame.time;
                }
                else startLifetime = curve.constantMax;
            
                ParticleSystem.MinMaxCurve delayCurve = main.startDelay;
                float delay;
                if (delayCurve.curveMax is { length: > 0 })
                {
                    Keyframe delayLastFrame = delayCurve.curveMax[delayCurve.curveMax.length-1];
                    delay = delayLastFrame.time;
                }
                else delay = delayCurve.constantMax;
            
                float duration = particleSystem.main.duration;
            
                lifetime = startLifetime + duration + delay;
            }
            
            
            SHRuntimeHelper.DoAfter(lifetime.Value, () =>
            {
                if(particleSystem != null) poolTuple.pool?.Release(particleSystem);
            });
            
            return particleSystem;
        }

        public static List<ParticleSystem> SpawnAll(List<ParticleSystem> prefabs, Vector3 position, Quaternion rotation = default, Transform parent = null, float emissionMultiplier = 1f, float scaleMultiplier = 1f, float? lifetime = null)
        {
            List<ParticleSystem> spawned = new();
            foreach (var prefab in prefabs)
            {
                spawned.Add(Spawn(prefab, position, rotation, parent, scaleMultiplier, lifetime));
            }

            return spawned;
        }
        
        public static ParticleSystem SpawnRandom(List<ParticleSystem> prefabs, Vector3 position, Quaternion rotation = default, Transform parent = null, float emissionMultiplier = 1f, float scaleMultiplier = 1f, float? lifetime = null)
        {
            return Spawn(prefabs[Random.Range(0, prefabs.Count)], position, rotation, parent, scaleMultiplier, lifetime);
        }
    }
}
