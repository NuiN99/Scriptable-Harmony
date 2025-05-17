using Random = UnityEngine.Random;

namespace NuiN.NExtensions
{
    public class TimerRandom : TimerBase
    {
        float _min;
        float _max;

        public TimerRandom(float min, float max, bool startCompleted = false) : base(startCompleted)
        {
            _min = min;
            _max = max;
            _duration = Random.Range(min, max);
        }

        public override float Duration
        {
            get => _duration;
            protected set => _duration = value;
        }
    
        float _duration;
        
        protected override void Initialize()
        {
            _duration = Random.Range(_min, _max);
        }
    }
}
