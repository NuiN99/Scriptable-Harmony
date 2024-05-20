using UnityEngine;

namespace NuiN.NExtensions
{
    public abstract class Timer
    {
        float _timeStart;
        bool _firstIteration = true;
        
        public abstract float Duration { get; protected set; }
        protected abstract bool StartCompleted { get; set; }
        
        public Timer SetDuration(float duration)
        {
            Duration = duration;
            return this;
        }

        public Timer Reset()
        {
            _timeStart = Time.time;
            _firstIteration = true;
            StartCompleted = true;
            return this;
        }

        public bool Complete()
        {
            float time = Time.time;

            if (_firstIteration)
            {
                Setup();
                _firstIteration = false;
                
                if (StartCompleted)
                {
                    StartCompleted = false;
                    return true;
                }
                
                _timeStart = time;
            }

            if (time - _timeStart <= Duration) return false;

            _timeStart = time;
            Setup();
            
            return true;
        }

        protected abstract void Setup();
    }
}
