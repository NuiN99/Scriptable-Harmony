using UnityEngine;
using UnityEngine.UI;

namespace NuiN.ScriptableHarmony.Sound
{
    public class GlobalVolumeSlider : MonoBehaviour
    {
        [SerializeField] Slider slider;

        void Reset() => slider = GetComponentInChildren<Slider>();

        void OnEnable()
        {
            slider.value = MasterVolumeManager.GetPrefsVolume();
            slider.onValueChanged.AddListener(MasterVolumeManager.SetVolume);
        }
        void OnDisable()
        {
            slider.onValueChanged.RemoveListener(MasterVolumeManager.SetVolume);
        }
    }
}