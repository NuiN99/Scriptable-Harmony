using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NuiN.ScriptableHarmony.Particles
{
    public static class ParticleSpawner
    {
        public static ParticleSystem Spawn(ParticleSystem prefab, Vector3 position, Quaternion rotation = default, Transform parent = null, float emissionMultiplier = 1f, float scaleMultiplier = 1f, float? lifetime = null)
        {
            ParticleSystem newSystem = Object.Instantiate(prefab, position, rotation, parent);
            
            if (lifetime == null)
            {
                ParticleSystem.MainModule main = newSystem.main;
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
            
                float duration = newSystem.main.duration;
            
                lifetime = startLifetime + duration + delay;
            }
            
            Object.Destroy(newSystem.gameObject, (float)lifetime);
            return newSystem;
        }

        public static List<ParticleSystem> SpawnAll(List<ParticleSystem> prefabs, Vector3 position, Quaternion rotation = default, Transform parent = null, float emissionMultiplier = 1f, float scaleMultiplier = 1f, float? lifetime = null)
        {
            List<ParticleSystem> spawned = new();
            foreach (var prefab in prefabs)
            {
                spawned.Add(Spawn(prefab, position, rotation, parent, emissionMultiplier, scaleMultiplier, lifetime));
            }

            return spawned;
        }
        
        public static ParticleSystem SpawnRandom(List<ParticleSystem> prefabs, Vector3 position, Quaternion rotation = default, Transform parent = null, float emissionMultiplier = 1f, float scaleMultiplier = 1f, float? lifetime = null)
        {
            return Spawn(prefabs[Random.Range(0, prefabs.Count)], position, rotation, parent, emissionMultiplier, scaleMultiplier, lifetime);
        }
    }
}
