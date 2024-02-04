using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NuiN.NExtensions
{
    [Serializable]
    public class RandomTimer : Timer
    {
        float _duration;
        
        [SerializeField, Tooltip("Minimum possible time before completion")]
        float minDuration;
        [SerializeField, Tooltip("Maximum possible time before completion")]
        float maxDuration;
        
        [SerializeField, Tooltip("Makes the timer return completed on the first call")]
        bool startCompleted = true;

        public override float Duration { get => _duration; protected set => _duration = value; }
        protected override bool StartCompleted { get => startCompleted; set => startCompleted = value; }

        public SimpleTimer(float minDuration, float maxDuration)
        {
            this.minDuration = minDuration;
            this.maxDuration = maxDuration;
        }

        protected override void Setup()
        {
            _duration = Random.Range(minDuration, maxDuration);
        }
    }
}