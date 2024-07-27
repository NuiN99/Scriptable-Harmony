using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace NuiN.NExtensions
{
    public static class RuntimeHelper
    {
        public static void SubOnLoad(Action action) => RuntimeHelperInstance.OnGameLoaded += action;
        public static void UnSubOnLoad(Action action) => RuntimeHelperInstance.OnGameLoaded -= action;
        
        public static void SubOnUpdate(Action action) => RuntimeHelperInstance.OnUpdate += action;
        public static void UnSubOnUpdate(Action action) => RuntimeHelperInstance.OnUpdate -= action;

        public static Coroutine StartCoroutine(IEnumerator coroutine) => RuntimeHelperInstance.MonoInstance.StartCoroutine(coroutine);
        
        public static Coroutine DoAfter(float seconds, Action onComplete) => RuntimeHelperInstance.MonoInstance.StartCoroutine(DoAfterRoutine(seconds, onComplete));

        static IEnumerator DoAfterRoutine(float seconds, Action onComplete)
        {
            yield return new WaitForSeconds(seconds);
            onComplete?.Invoke();
        }
    }
    
    internal class RuntimeHelperInstance : MonoBehaviour
    {
        internal static event Action OnGameLoaded = delegate { };
        internal static event Action OnUpdate = delegate { };

        static RuntimeHelperInstance instance;
        public static MonoBehaviour MonoInstance { get; private set; }
   
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InvokeGameLoadedEvent()
        {
            instance = new GameObject("Scriptable Harmony Runtime Helper").AddComponent<RuntimeHelperInstance>();
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

        void Update()
        {
            OnUpdate.Invoke();
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

