using UnityEngine;

namespace NuiN.ScriptableHarmony.Sound
{
    public class SpatialSound : MonoBehaviour
    {
        SHSource _source;
        
        [SerializeField] SoundSO sound;
        [SerializeField] Transform soundPosition;

        [SerializeField] float volumeMult = 1f;
        [SerializeField] float pitchMult = 1f;

        void Reset()
        {
            soundPosition = transform;
        }

        void Start()
        {
            if (soundPosition == null) soundPosition = transform;
            _source = sound.PlaySpatial(soundPosition.position, soundPosition, volumeMult, pitchMult);
        }

        void OnDestroy()
        {
            sound.SoundPlayer.ReleaseToPool(_source);
        }
    }
}