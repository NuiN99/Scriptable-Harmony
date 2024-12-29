using System;
using UnityEngine;

namespace NuiN.NExtensions
{
    [Serializable]
    public abstract class TimerBase
    {
        public bool IsComplete => Time >= Duration;
        public float Time => Mathf.Clamp(UnityEngine.Time.time - _startTime, 0, Duration);
        public float Progress => Time / Duration;

        public abstract float Duration { get; protected set; }

        [SerializeField] protected bool startCompleted;

        bool _hasCompletedOnce;
        float _startTime;

        public TimerBase(bool startCompleted = false)
        {
            this.startCompleted = startCompleted;
            Restart();
        }
        
        protected virtual void Initialize() { }

        public void Restart()
        {
            _startTime = UnityEngine.Time.time;

            Initialize();
            
            if (startCompleted && !_hasCompletedOnce)
            {
                _hasCompletedOnce = true;
                _startTime = float.MinValue;
            }
        }
    }
}