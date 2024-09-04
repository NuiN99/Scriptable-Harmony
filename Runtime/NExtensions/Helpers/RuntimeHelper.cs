using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace NuiN.NExtensions
{
    public class RuntimeHelper : MonoBehaviour
    {
        public static event Action OnGameLoadedEvent = delegate { };
        public static event Action UpdateEvent = delegate { };
        public static event Action OnDrawGizmosEvent = delegate { };
        
        static RuntimeHelper instance;
        public static RuntimeHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<RuntimeHelper>();
                    
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
        static void InvokeGameLoadedEvent() => OnGameLoadedEvent.Invoke();

        static RuntimeHelper CreateInstance()
        {
            RuntimeHelper obj = new GameObject("Scriptable Harmony Runtime Helper").AddComponent<RuntimeHelper>();
            if(Application.isPlaying) DontDestroyOnLoad(obj);
            return obj;
        }
        
        static IEnumerator DoAfterRoutine(float seconds, Action onComplete)
        {
            yield return new WaitForSeconds(seconds);
            onComplete?.Invoke();
        }

        void Update() => UpdateEvent.Invoke();
        void OnDrawGizmos() => OnDrawGizmosEvent.Invoke();
    }
}

