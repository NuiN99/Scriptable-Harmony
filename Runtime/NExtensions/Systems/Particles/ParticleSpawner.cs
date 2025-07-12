using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace NuiN.NExtensions
{
    public static class ParticleSpawner
    {
        static Dictionary<ParticleSystem, (UnityEngine.Pool.ObjectPool<ParticleSystem> pool, Transform container)> prefabObjectPools = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void ResetObjectPoolDictionary()
        {
            prefabObjectPools = new Dictionary<ParticleSystem, (UnityEngine.Pool.ObjectPool<ParticleSystem> pool, Transform container)>();

            SceneManager.sceneLoaded -= ResetObjectPoolDictionaryOnSceneLoad;
            SceneManager.sceneLoaded += ResetObjectPoolDictionaryOnSceneLoad;
        }

        static void ResetObjectPoolDictionaryOnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            ResetObjectPoolDictionary();
        }

        static (UnityEngine.Pool.ObjectPool<ParticleSystem> pool, Transform container) CreatePool(ParticleSystem prefab)
        {
            Transform container = new GameObject($"{prefab.name} | ObjectPool").transform;

            var pool = new UnityEngine.Pool.ObjectPool<ParticleSystem>(
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

            if (!prefabObjectPools.TryGetValue(prefab, out (UnityEngine.Pool.ObjectPool<ParticleSystem> pool, Transform container) poolTuple))
            {
                poolTuple = CreatePool(prefab);
                prefabObjectPools.Add(prefab, poolTuple);
            }

            ParticleSystem particleSystem = poolTuple.pool.Get();

            if (parent != null)
            {
                var main = particleSystem.main;
                main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            }

            particleSystem.transform.SetParent(parent, worldPositionStays: true);
            particleSystem.transform.SetPositionAndRotation(position, rot);
            particleSystem.transform.localScale = prefab.transform.localScale * scaleMultiplier;

            if (lifetime == null)
            {
                ParticleSystem.MainModule main = particleSystem.main;
                ParticleSystem.MinMaxCurve curve = main.startLifetime;
                float startLifetime;
                if (curve.curveMax is { length: > 0 })
                {
                    Keyframe lastFrame = curve.curveMax[curve.curveMax.length - 1];
                    startLifetime = lastFrame.time;
                }
                else startLifetime = curve.constantMax;

                ParticleSystem.MinMaxCurve delayCurve = main.startDelay;
                float delay;
                if (delayCurve.curveMax is { length: > 0 })
                {
                    Keyframe delayLastFrame = delayCurve.curveMax[delayCurve.curveMax.length - 1];
                    delay = delayLastFrame.time;
                }
                else delay = delayCurve.constantMax;

                float duration = particleSystem.main.duration;

                lifetime = startLifetime + duration + delay;
            }


            RuntimeHelper.DoAfter(lifetime.Value, () =>
            {
                if (particleSystem != null) poolTuple.pool?.Release(particleSystem);
            });

            return particleSystem;
        }

        public static List<ParticleSystem> SpawnAll(List<ParticleSystem> prefabs, Vector3 position, Quaternion? rotation = null, Transform parent = null, float scaleMultiplier = 1f, float? lifetime = null)
        {
            List<ParticleSystem> spawned = new();
            foreach (var prefab in prefabs)
            {
                spawned.Add(Spawn(prefab, position, rotation, parent, scaleMultiplier, lifetime));
            }

            return spawned;
        }

        public static ParticleSystem SpawnRandom(List<ParticleSystem> prefabs, Vector3 position, Quaternion? rotation = null, Transform parent = null, float scaleMultiplier = 1f, float? lifetime = null)
        {
            return Spawn(prefabs[Random.Range(0, prefabs.Count)], position, rotation, parent, scaleMultiplier, lifetime);
        }
    }
}
