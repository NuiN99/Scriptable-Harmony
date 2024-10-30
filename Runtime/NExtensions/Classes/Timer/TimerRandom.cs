using System;
using NuiN.NExtensions;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class TimerRandom : TimerBase
{
    [SerializeField] float min;
    [SerializeField] float max;

    public TimerRandom(float min, float max, bool startCompleted) : base(startCompleted)
    {
        this.min = min;
        this.max = max;
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
        _duration = Random.Range(min, max);
    }
}