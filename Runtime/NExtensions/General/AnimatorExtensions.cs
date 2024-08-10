using System;
using System.Collections;
using System.Collections.Generic;
using NuiN.SpleenTween;
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

        public static bool Complete(this Animator animator)
        {
            return animator.NormalizedTime() >= 1;
        }

        public static float CurrentStateDuration(this Animator animator, int layerIndex = 0)
        {
            return animator.GetCurrentAnimatorStateInfo(layerIndex).length;
        }
        
        /// <summary>
        /// Remember to wait for crossfade time before calling
        /// </summary>
        public static Coroutine OnReachedNormalizedTime(this Animator animator, MonoBehaviour monoBehaviour, float time, Action action)
        {
            return monoBehaviour.DoWhen(() => animator.NormalizedTime() >= time, action);
        }
    }
}