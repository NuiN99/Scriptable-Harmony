using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NuiN.NExtensions
{
    [Serializable]
    public class Timer
    {
        float _timeStart;
    
        [SerializeField, Tooltip("Automatically set random duration upon completion")]
        bool randomDuration;

        [SerializeField, ShowIf(nameof(randomDuration), true), Tooltip("Min possible time before the timer completes")]
        float minDuration;
        
        [SerializeField, ShowIf(nameof(randomDuration), true), Tooltip("Max possible time before the timer completes")]
        float maxDuration;

        [SerializeField, ShowIf(nameof(randomDuration), false), Tooltip("How long before the timer completes")] 
        float duration;
        
        [SerializeField, Tooltip("Makes the timer return completed on the first call")] 
        bool startCompleted = true;

        public Timer InitRandom()
        {
            duration = Random.Range(minDuration, maxDuration);
            return this;
        }

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

            if(randomDuration) InitRandom();
            return true;
        }
    }
}


