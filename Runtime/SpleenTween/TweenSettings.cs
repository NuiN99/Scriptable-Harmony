using System;
using System.Collections;
using System.Collections.Generic;
using NuiN.NExtensions;
using UnityEngine;

namespace NuiN.SpleenTween
{
    [Serializable]
    public class TweenSettings
    {
        [SerializeField] float duration = 1f;
        [SerializeField] Ease ease = Ease.Custom;

        [SerializeField, ShowIf(nameof(ease), (int)Ease.Custom)] AnimationCurve customEase = AnimationCurve.Linear(0, 0, 1, 1);

        public float Duration => duration;
        public Ease Ease => ease;
        public AnimationCurve CustomEase => customEase;
    }
}