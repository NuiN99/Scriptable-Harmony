using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace NuiN.NExtensions
{
    public static class RuntimeHelper
    {
        public static event Action OnGameLoaded = delegate { };
        public static event Action OnUpdate = delegate { };
        public static event Action OnDrawGizmos = delegate { };

        public static Coroutine StartCoroutine(IEnumerator coroutine) => RuntimeHelperInstance.MonoInstance.StartCoroutine(coroutine);
        
        public static Coroutine DoAfter(float seconds, Action onComplete) => RuntimeHelperInstance.MonoInstance.StartCoroutine(DoAfterRoutine(seconds, onComplete));

        static IEnumerator DoAfterRoutine(float seconds, Action onComplete)
        {
            yield return new WaitForSeconds(seconds);
            onComplete?.Invoke();
        }

        internal static void InvokeOnGameLoaded() => OnGameLoaded.Invoke();
        internal static void InvokeOnUpdate() => OnUpdate.Invoke();
        internal static void InvokeOnDrawGizmos() => OnDrawGizmos.Invoke();
    }
    
    internal class RuntimeHelperInstance : MonoBehaviour
    {
        static RuntimeHelperInstance instance;
        public static MonoBehaviour MonoInstance { get; private set; }
   
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InvokeGameLoadedEvent()
        {
            instance = new GameObject("Scriptable Harmony Runtime Helper").AddComponent<RuntimeHelperInstance>();
            MonoInstance = instance;
            RuntimeHelper.InvokeOnGameLoaded();
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

        void Update() => RuntimeHelper.InvokeOnUpdate();
        void OnDrawGizmos() => RuntimeHelper.InvokeOnDrawGizmos();

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

