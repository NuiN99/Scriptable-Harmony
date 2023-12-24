using UnityEngine;

namespace NuiN.ScriptableHarmony.Sound
{
    [CreateAssetMenu(menuName = "ScriptableHarmony/Sound/Sound Array", fileName = "New Sound Array")]
    public class SoundArraySO : SoundBaseSO
    {
        [SerializeField] AudioClip[] audioClips;

        public AudioClip[] Clips => audioClips;

        public void PlayRandom(float volumeMult = 1f)
            => player.PlayRandom(this, volumeMult);
        public void PlayRandomSpatial(Vector3 position, float volumeMult = 1f, Transform parent = null)
            => player.PlayRandomSpatial(this, position, volumeMult, parent);

        public void PlayIndex(int index, float volumeMult = 1)
            => player.PlayIndex(this, index, volume * volumeMult);
        public void PlayIndexSpatial(int index, Vector3 position, float volumeMult = 1f, Transform parent = null)
            => player.PlayIndexSpatial(this, index, position, volumeMult, parent);

        public void PlayAll(float volumeMult = 1)
            => player.PlayAll(this, volume * volumeMult);
        public void PlayAllSpatial(Vector3 position, float volumeMult = 1, Transform parent = null)
            => player.PlayAllSpatial(this, position, volume * volumeMult, parent);
        
        public static SoundArraySO CreateInstance(AudioClip[] clips, SoundPlayerSO soundPlayer)
        {
            var obj = CreateInstance<SoundArraySO>();
            obj.audioClips = clips;
            obj.player = soundPlayer;
            return obj;
        }
    }
}