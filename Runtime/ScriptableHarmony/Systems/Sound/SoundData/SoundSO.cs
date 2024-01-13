using UnityEngine;

namespace NuiN.ScriptableHarmony.Sound
{
    [CreateAssetMenu(menuName = "ScriptableHarmony/Sound/Sound Object", fileName = "New Sound")]
    public class SoundSO : ScriptableObject
    {
        [SerializeField] SoundPlayerSO player;
        [SerializeField] SoundSettings settings;
        
        void Reset() => player = Resources.Load<SoundPlayerSO>("Default Sound Player");

        // ReSharper disable Unity.PerformanceAnalysis 
        public void Play()
        {
            if (!ClipsAreValid()) return;
            player.Play(settings);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void PlaySpatial(Vector3 position, Transform parent = null)
        {
            if (!ClipsAreValid()) return;
            player.PlaySpatial(settings, position, parent);
        }

        bool ClipsAreValid()
        {
            if (settings.Clips.Length != 0) return true;
            Debug.LogError("SoundSO had no clips when attempting to play", this);
            return false;
        }
        
        public static SoundSO CreateInstance(AudioClip[] clips, SoundPlayerSO soundPlayer)
        {
            var obj = ScriptableObject.CreateInstance<SoundSO>();
            obj.settings.SetClips(clips);
            obj.player = soundPlayer;
            return obj;
        }
    }
}