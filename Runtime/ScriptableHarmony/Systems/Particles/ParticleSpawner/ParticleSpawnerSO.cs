using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NuiN.ScriptableHarmony.Particles
{
    [CreateAssetMenu(menuName = "ScriptableHarmony/Particles/Spawner", fileName = "New Particle Spawner")]
    public class ParticleSpawnerSO : ScriptableObject
    {
        [Header("Options")]
        [SerializeField] float globalEmissionMultiplier = 1f;
        [SerializeField] float globalScaleMultiplier = 1f;
        [SerializeField] bool disableParticles;
        
        public ParticleSystem Spawn(ParticleSystem particleSystem, Vector3 position, Quaternion rotation = default, Transform parent = null, float emissionMultiplier = 1f, float scaleMultiplier = 1f, float? lifetime = null)
        {
            if (disableParticles) return null;
            ParticleSystem newSystem = Instantiate(particleSystem, position, rotation, parent);
            var emission = newSystem.emission;
            
            newSystem.transform.localScale *= scaleMultiplier * globalScaleMultiplier;
            emission.rateOverTimeMultiplier *= emissionMultiplier * globalEmissionMultiplier;

            if (lifetime == null)
            {
                ParticleSystem.MainModule main = newSystem.main;
                ParticleSystem.MinMaxCurve curve = main.startLifetime;
                float startLifetime;
                if (curve.curveMax != null && curve.curveMax.length > 0)
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
            
            Destroy(newSystem.gameObject, (float)lifetime);
            return newSystem;
        }
        
        public void SpawnRandom(List<ParticleSystem> particleSystems, Vector3 position, Quaternion rotation = default, Transform parent = null, float emissionMultiplier = 1f, float scaleMultiplier = 1f, float? lifetime = null)
        {
            if (disableParticles) return;
            
            ParticleSystem randSystem = particleSystems[Random.Range(0, particleSystems.Count)];
            Spawn(randSystem, position, rotation, parent, emissionMultiplier, scaleMultiplier, lifetime);
        }
    }
}
