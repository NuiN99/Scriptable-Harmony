using UnityEngine;

namespace NuiN.ScriptableHarmony.Sound
{
    [CreateAssetMenu(menuName = "ScriptableHarmony/Sound/Sound Object", fileName = "New Sound")]
    public class SoundSO : SoundBaseSO
    {
        [SerializeField] AudioClip audioClip;

        public AudioClip Clip => audioClip;

        public void Play(float volumeMult = 1f)
            => player.Play(this, volumeMult);
        public void PlaySpatial(Vector3 position, float volumeMult = 1f, Transform parent = null)
            => player.PlaySpatial(this, position, volumeMult, parent);
        
        public static SoundSO CreateInstance(AudioClip clip, SoundPlayerSO soundPlayer)
        {
            var obj = CreateInstance<SoundSO>();
            obj.audioClip = clip;
            obj.player = soundPlayer;
            return obj;
        }
    }
}