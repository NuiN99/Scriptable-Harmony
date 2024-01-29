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
            if(instance != null) Destroy(instance.gameObject); 
            instance = new GameObject("Scriptable Harmony Sounds Runtime").AddComponent<SHSoundsRuntime>();
            
            instance._audioSources.SetToResource("SH_Sources");
        }

        void Awake()
        {
            if (instance != null && instance != this) Destroy(gameObject);
        }

        void Update()
        {
            foreach (SHSource source in _audioSources) source.UpdateVolume();
        }
    }
}

