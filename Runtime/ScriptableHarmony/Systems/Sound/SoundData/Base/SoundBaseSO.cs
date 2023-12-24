using UnityEngine;

namespace NuiN.ScriptableHarmony.Sound
{
    public abstract class SoundBaseSO : ScriptableObject
    {
        [SerializeField] protected SoundPlayerSO player;
        [SerializeField] [Range(0, 1)] protected float volume = 1f;

        public SoundPlayerSO SoundPlayer => player;
        public float Volume => volume;

        void Reset()
        {
            player = Resources.Load<SoundPlayerSO>("Default Sound Player");
        }
    }
}