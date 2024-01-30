using System;
using UnityEngine;

namespace NuiN.NExtensions
{
    [Serializable]
    public class Timer
    {
        float _timeStart;
    
        [SerializeField, Tooltip("How long before the timer completes")] 
        float duration;
        
        [SerializeField, Tooltip("Makes the timer return completed on the first call")] 
        bool startCompleted = true;

        public Timer SetDuration(float duration)
        {
            this.duration = duration;
            return this;
        }

        public bool Complete()
        {
            float time = Time.time;
            
            if (startCompleted)
            {
                _timeStart = time;
                startCompleted = false;
                return true;
            }

            if (time - _timeStart <= duration) return false;
        
            _timeStart = time;
            return true;
        }
    }
}


