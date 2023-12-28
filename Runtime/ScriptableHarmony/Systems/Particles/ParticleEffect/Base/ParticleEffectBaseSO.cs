using NuiN.ScriptableHarmony.References;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Particles
{
    public abstract class ParticleEffectBaseSO : ScriptableObject
    {
        [SerializeField] protected ParticleSpawnerSO spawner;
        [SerializeField] protected float emissionMultiplier = 1f;
        [SerializeField] protected float scaleMultiplier = 1f;
        
        public ParticleSpawnerSO ParticleSpawner => spawner;
        public float EmissionMultiplier => emissionMultiplier;
        public float ScaleMultiplier => scaleMultiplier;
        
        protected abstract ParticleSystem GetParticleSystem();

        public void Spawn(Vector3 position, Quaternion rotation = default, Transform parent = null, float emissionFactor = 1f, float scaleFactor = 1f, float? lifetime = null)
            => spawner.Spawn(GetParticleSystem(), position, rotation, parent, emissionMultiplier * emissionFactor, scaleMultiplier * scaleFactor, lifetime);
    }
}