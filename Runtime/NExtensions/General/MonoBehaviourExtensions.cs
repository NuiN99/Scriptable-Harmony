using System;
using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void TryStopCoroutine(this MonoBehaviour behaviour, Coroutine coroutine)
    {
        if(coroutine != null) behaviour.StopCoroutine(coroutine);
    }
    
    public static Coroutine ReplaceCoroutine(this MonoBehaviour behaviour, Coroutine coroutine, IEnumerator enumerator)
    {
        behaviour.TryStopCoroutine(coroutine);
        return behaviour.StartCoroutine(enumerator);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public static Coroutine DoAfter(this MonoBehaviour behaviour, float seconds, Action onComplete)
    {
        return behaviour.StartCoroutine(DoAfterCoroutine(seconds, onComplete));
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public static Coroutine DoWhen(this MonoBehaviour behaviour, Func<bool> condition, Action onComplete)
    {
        return behaviour.StartCoroutine(DoWhenCoroutine(condition, onComplete));
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public static Coroutine DoFor(this MonoBehaviour behaviour, float seconds, Action onUpdate, Action onComplete = null, Func<bool> stopIf = null, Action onStop = null)
    {
        return behaviour.StartCoroutine(DoForCoroutine(seconds, onUpdate, onComplete, stopIf, onStop));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    static IEnumerator DoAfterCoroutine(float seconds, Action onComplete)
    {
        yield return new WaitForSeconds(seconds);
        onComplete?.Invoke();
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    static IEnumerator DoWhenCoroutine(Func<bool> condition, Action onComplete)
    {
        yield return new WaitUntil(condition);
        onComplete?.Invoke();
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    static IEnumerator DoForCoroutine(float seconds, Action onUpdate, Action onComplete, Func<bool> stopIf, Action onStop)
    {
        bool nullCondition = stopIf == null;
        
        float time = 0f;
        while (time < seconds)
        {
            if (!nullCondition && stopIf.Invoke())
            {
                onStop?.Invoke();
                yield break;
            }
            
            time += Time.deltaTime;
            onUpdate?.Invoke();
            yield return null;
        }
        
        onComplete?.Invoke();
    }
}
