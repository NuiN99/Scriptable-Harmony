using System;
using UnityEditor;
using UnityEngine;

namespace NuiN.NExtensions
{
    [Serializable]
    public class Timer : TimerBase
    {
        public Timer(float duration)
        {
            this.duration = duration;
        }

        public override float Duration
        {
            get => duration;
            protected set => duration = value;
        }
        
        [SerializeField] float duration;
    }
}