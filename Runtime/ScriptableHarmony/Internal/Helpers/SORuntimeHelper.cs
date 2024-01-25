using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    internal static class SORuntimeHelper
    {
        public static void SubOnLoad(Action action) => SORuntimeHelperInstance.OnGameLoaded += action;
        public static void UnSubOnLoad(Action action) => SORuntimeHelperInstance.OnGameLoaded -= action;

        public static Coroutine StartCoroutine(IEnumerator coroutine) => SORuntimeHelperInstance.MonoInstance.StartCoroutine(coroutine);
        
        public static Coroutine DoAfter(float seconds, Action onComplete) => SORuntimeHelperInstance.MonoInstance.StartCoroutine(DoAfterRoutine(seconds, onComplete));

        static IEnumerator DoAfterRoutine(float seconds, Action onComplete)
        {
            yield return new WaitForSeconds(seconds);
            onComplete?.Invoke();
        }
    }
    
    internal class SORuntimeHelperInstance : MonoBehaviour
    {
        internal static event Action OnGameLoaded = delegate { };

        static SORuntimeHelperInstance instance;
        public static MonoBehaviour MonoInstance { get; private set; }
   
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InvokeGameLoadedEvent()
        {
            instance = new GameObject("Scriptable Harmony Runtime Helper").AddComponent<SORuntimeHelperInstance>();
            MonoInstance = instance;
            OnGameLoaded.Invoke();
        }

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            MonoInstance = this;
            
            DontDestroyOnLoad(gameObject);
        }

        #if UNITY_EDITOR
        static void UnloadInstance(PlayModeStateChange newMode)
        {
            if (newMode != PlayModeStateChange.EnteredEditMode) return;
            
            MonoInstance.StopAllCoroutines();
            Destroy(instance.gameObject);
        }
        void OnEnable() => EditorApplication.playModeStateChanged += UnloadInstance;
        void OnDisable() => EditorApplication.playModeStateChanged -= UnloadInstance;
        #endif
    }
}

