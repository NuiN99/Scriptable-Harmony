using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Sound
{
    public static class MasterVolumeManager
    {
        public static float GlobalVolume { get; private set; }
        public const string GLOBAL_VOLUME_KEY = "SH_GlobalVolume";
    
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            SetVolume(GetPrefsVolume());
        }
    
        public static void SetVolume(float newVolume)
        {
            GlobalVolume = Mathf.Clamp01(newVolume);
            PlayerPrefs.SetFloat(GLOBAL_VOLUME_KEY, GlobalVolume);
        }
        
        public static float GetPrefsVolume()
        {
            return PlayerPrefs.HasKey(GLOBAL_VOLUME_KEY)
                ? PlayerPrefs.GetFloat(GLOBAL_VOLUME_KEY)
                : 1f;
        }

        #if UNITY_EDITOR
        internal static void DrawSliderGUI()
        {
            SetVolume(EditorGUILayout.Slider("Global Volume", GetPrefsVolume(), 0f, 1f));
        }
        #endif
    }
}
