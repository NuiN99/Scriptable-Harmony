using UnityEngine;

namespace NuiN.NExtensions
{
    public abstract class Timer
    {
        float _timeStart;
        bool _firstIteration = true;
        bool _startCompleted;
        
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
            return this;
        }

        public bool Complete()
        {
            float time = Time.time;

            if (_firstIteration)
            {
                Setup();
                _startCompleted = StartCompleted;
                _firstIteration = false;
                
                if (_startCompleted)
                {
                    _startCompleted = false;
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
