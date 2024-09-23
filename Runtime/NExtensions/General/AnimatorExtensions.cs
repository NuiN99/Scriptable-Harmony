using System;
using System.Collections;
using UnityEngine;

namespace NuiN.NExtensions
{
    public static class AnimatorExtensions
    {
        public static float NormalizedTime(this Animator animator)
        {
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            return Mathf.Min(normalizedTime, 1);
        }

        public static bool IsComplete(this Animator animator)
        {
            return animator.NormalizedTime() >= 1;
        }

        public static float CurrentStateDuration(this Animator animator, int layerIndex = 0)
        {
            // update the animator to make sure current state is correct
            animator.Update(0f);
            return animator.GetCurrentAnimatorStateInfo(layerIndex).length;
        }
        
        /// <summary>
        /// Remember to wait for crossfade time before calling
        /// </summary>
        public static Coroutine OnReachedNormalizedTime(this Animator animator, MonoBehaviour monoBehaviour, float time, Action action)
        {
            return monoBehaviour.DoWhen(() => animator.NormalizedTime() >= time, action);
        }

        public static Coroutine OnChangeState(this Animator animator, MonoBehaviour monoBehaviour, Action action, int layerIndex = 0)
        {
            // update the animator to make sure current state is correct
            animator.Update(0f);
            
            AnimatorStateInfo initialState = animator.GetCurrentAnimatorStateInfo(layerIndex);
            
            return monoBehaviour.StartCoroutine(OnChangeStateRoutine(animator, initialState, action, layerIndex));
        }

        static IEnumerator OnChangeStateRoutine(Animator animator, AnimatorStateInfo initialState, Action action, int layerIndex)
        {
            yield return null;
            while (animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash == initialState.fullPathHash)
            {
                yield return null;
            }
            
            action?.Invoke();
        }
    }
}