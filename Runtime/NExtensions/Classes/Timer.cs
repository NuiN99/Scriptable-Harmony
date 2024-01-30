using System;
using UnityEngine;

namespace NuiN.NExtensions
{
    [Serializable]
    public class Timer
    {
        float _timeStart;
    
        [SerializeField] float duration;
        [SerializeField] bool startCompleted = true;

        public Timer SetDuration(float duration)
        {
            this.duration = duration;
            return this;
        }

        public bool Complete()
        {
            if (startCompleted)
            {
                _timeStart = Time.time;
                startCompleted = false;
                return true;
            }

            if (Time.time - _timeStart <= duration) return false;
        
            _timeStart = Time.time;
            return true;
        }
    }
}


