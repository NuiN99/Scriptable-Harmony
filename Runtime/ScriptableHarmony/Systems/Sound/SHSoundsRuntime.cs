using UnityEngine;

namespace NuiN.ScriptableHarmony.Sound
{
    public class SHSoundsRuntime : MonoBehaviour
    {
        static SHSoundsRuntime instance;

        SetRuntimeSet<SHSource> _audioSources = new();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            SHSoundsRuntime shSounds = new GameObject("Scriptable Harmony Sounds Runtime").AddComponent<SHSoundsRuntime>();
            shSounds._audioSources.SetToResource("SH_Sources");
        }

        void Awake()
        {
            if (instance != null && instance != this) 
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        void Update()
        {
            foreach (SHSource source in _audioSources) source.UpdateVolume();
        }
    }
}

