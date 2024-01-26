using UnityEngine;

namespace NuiN.ScriptableHarmony.Particles
{
    [CreateAssetMenu(menuName = "ScriptableHarmony/Particles/Particle Effect", fileName = "New Particle Effect")]
    public class ParticleEffectSO : ParticleEffectBaseSO
    {
        [SerializeField] ParticleSystem particleSystem;
        public ParticleSystem ParticleSystem => particleSystem;
        
        protected override ParticleSystem GetParticleSystem()
            => particleSystem;
    }
}