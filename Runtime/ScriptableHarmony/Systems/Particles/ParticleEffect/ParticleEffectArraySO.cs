using NuiN.ScriptableHarmony.References;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Particles
{
    [CreateAssetMenu(menuName = "ScriptableHarmony/Particles/Particle Effect Array", fileName = "New Particle Effect Array")]
    public class ParticleEffectArraySO : ParticleEffectBaseSO
    {
        [SerializeField] ParticleSystem[] particleSystems;
        public ParticleSystem[] ParticleSystems => particleSystems;
        
        protected override ParticleSystem GetParticleSystem()
            => particleSystems[Random.Range(0, particleSystems.Length)];

        public void SpawnIndex(int index, Vector3 position, Quaternion rotation = default, Transform parent = null, float emissionFactor = 1f, float scaleFactor = 1f, float? lifetime = null)
            => spawner.Spawn(particleSystems[index], position, rotation, parent, emissionMultiplier * emissionFactor, scaleMultiplier * scaleFactor, lifetime);
        
        public void SpawnAll(Vector3 position, Quaternion rotation = default, Transform parent = null, float emissionFactor = 1f, float scaleFactor = 1f, float? lifetime = null)
        {
            foreach(var system in particleSystems) spawner.Spawn(system, position, rotation, parent, emissionMultiplier * emissionFactor, scaleMultiplier * scaleFactor, lifetime);
        }
    }
}