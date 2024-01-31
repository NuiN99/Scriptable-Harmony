using UnityEngine;

namespace NuiN.NExtensions
{
    internal abstract class Timer
    {
        float _timeStart;
        bool _firstIteration;
        
        public abstract float Duration { get; protected set; }
        protected abstract bool StartCompleted { get; set; }
        
        public Timer SetDuration(float duration)
        {
            Duration = duration;
            return this;
        }

        public bool Complete()
        {
            float time = Time.time;
            
            if(_firstIteration) Setup();
            
            if (StartCompleted)
            {
                _timeStart = time;
                StartCompleted = false;
                return true;
            }

            if (time - _timeStart <= Duration) return false;

            _timeStart = time;

            Setup();
            
            return true;
        }

        protected abstract void Setup();
    }
}
