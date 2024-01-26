using System.Collections.Generic;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Particles
{
    [CreateAssetMenu(menuName = "ScriptableHarmony/Particles/New Particle Spawner")]
    public class ParticleSpawnerSO : ScriptableObject
    {
        [SerializeField] List<ParticleSystem> particlePrefabs;

        public List<ParticleSystem> Spawn(Vector3 position, Quaternion? rotation = null, Transform parent = null, float scaleMultiplier = 1f, float? lifetime = null)
        {
            return particlePrefabs.Count <= 0 
                ? new List<ParticleSystem>() 
                : ParticleSpawnerManager.SpawnAll(particlePrefabs, position, rotation, parent, scaleMultiplier, lifetime);
        }

        public ParticleSystem SpawnRandom(Vector3 position, Quaternion? rotation = null, Transform parent = null, float scaleMultiplier = 1f, float? lifetime = null)
        {
            return particlePrefabs.Count <= 0 
                ? new ParticleSystem() 
                : ParticleSpawnerManager.SpawnRandom(particlePrefabs, position, rotation, parent, scaleMultiplier, lifetime);
        }
    }
}