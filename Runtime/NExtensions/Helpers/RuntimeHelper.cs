using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace NuiN.NExtensions
{
    public static class RuntimeHelper
    {
        public static event Action OnGameLoaded
        {
            add => RuntimeHelperInstance.Instance.onGameLoaded += value;
            remove => RuntimeHelperInstance.Instance.onGameLoaded -= value;
        }
        
        public static event Action OnUpdate
        {
            add => RuntimeHelperInstance.Instance.onUpdate += value;
            remove => RuntimeHelperInstance.Instance.onUpdate -= value;
        }
        
        public static event Action OnDrawGizmos
        {
            add => RuntimeHelperInstance.Instance.onDrawGizmos += value;
            remove => RuntimeHelperInstance.Instance.onDrawGizmos -= value;
        }

        public static Coroutine StartCoroutine(IEnumerator coroutine) => RuntimeHelperInstance.Instance.StartCoroutine(coroutine);
        
        public static Coroutine DoAfter(float seconds, Action onComplete) => RuntimeHelperInstance.Instance.StartCoroutine(DoAfterRoutine(seconds, onComplete));

        static IEnumerator DoAfterRoutine(float seconds, Action onComplete)
        {
            yield return new WaitForSeconds(seconds);
            onComplete?.Invoke();
        }
    }
    
    public class RuntimeHelperInstance : MonoBehaviour
    {
        internal Action onGameLoaded = delegate { };
        internal Action onUpdate = delegate { };
        internal Action onDrawGizmos = delegate { };
        
        static RuntimeHelperInstance instance;
        internal static RuntimeHelperInstance Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<RuntimeHelperInstance>();
                    
                    if (instance == null)
                    {
                        instance = CreateInstance();
                    }
                }

                return instance;
            }
        }

        void Awake()
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
   
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InvokeGameLoadedEvent() => Instance.onGameLoaded.Invoke();

        static RuntimeHelperInstance CreateInstance()
        {
            RuntimeHelperInstance obj = new GameObject("Scriptable Harmony Runtime Helper").AddComponent<RuntimeHelperInstance>();
            if(Application.isPlaying) DontDestroyOnLoad(obj);
            return obj;
        }

        void Update() => Instance.onUpdate.Invoke();
        void OnDrawGizmos() => Instance.onDrawGizmos.Invoke();

#if UNITY_EDITOR
        static void UnloadInstance(PlayModeStateChange newMode)
        {
            if (newMode != PlayModeStateChange.EnteredEditMode) return;
            
            Instance.StopAllCoroutines();
            Destroy(Instance.gameObject);
        }
        void OnEnable() => EditorApplication.playModeStateChanged += UnloadInstance;
        void OnDisable() => EditorApplication.playModeStateChanged -= UnloadInstance;
        #endif
    }
}

