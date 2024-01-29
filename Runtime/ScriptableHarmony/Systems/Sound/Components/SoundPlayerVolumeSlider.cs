using UnityEngine;
using UnityEngine.UI;

namespace NuiN.ScriptableHarmony.Sound
{
    public class SoundPlayerVolumeSlider : MonoBehaviour
    {
        [SerializeField] SoundPlayerSO soundPlayer;
        [SerializeField] Slider slider;

        void Reset() => slider = GetComponentInChildren<Slider>();

        void OnEnable()
        {
            slider.value = soundPlayer.GetPrefsVolume();
            slider.onValueChanged.AddListener(soundPlayer.SetVolume);
        }
        void OnDisable()
        {
            slider.onValueChanged.RemoveListener(soundPlayer.SetVolume);
        }

        void LateUpdate()
        {
            slider.value = soundPlayer.Volume;
        }
    }
}