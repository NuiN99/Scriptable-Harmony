using System;
using UnityEngine;

namespace NuiN.NExtensions
{
    [Serializable]
    public class SimpleTimer : Timer
    {
        [SerializeField, Tooltip("How long before the timer completes")]
        float duration;
        
        [SerializeField, Tooltip("Makes the timer return completed on the first call")]
        bool startCompleted = true;

        public override float Duration { get => duration; protected set => duration = value; }
        protected override bool StartCompleted { get => startCompleted; set => startCompleted = value; }

        public SimpleTimer(float duration)
        {
            this.duration = duration;
        }
        
        protected override void Setup() { }
    }
}