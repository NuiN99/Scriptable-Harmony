using System;
using System.Collections;
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
}

