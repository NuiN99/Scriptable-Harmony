using UnityEngine;

namespace NuiN.NExtensions
{
    public class Timer : TimerBase
    {
        public Timer(float duration, bool startCompleted = false) : base(startCompleted)
        {
            _duration = duration;
        }

        public override float Duration
        {
            get => _duration;
            protected set => _duration = value;
        }
        
        float _duration;
    }
}