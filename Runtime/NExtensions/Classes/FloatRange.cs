using System;
using UnityEngine;

namespace NuiN.NExtensions
{
    [Serializable]
    public class FloatRange
    {
        [field: SerializeField] public float Min { get; private set; }
        [field: SerializeField] public float Max { get; private set; }

        public float Lerp(float lerp) => Mathf.Lerp(Min, Max, lerp);
        public float Random() => UnityEngine.Random.Range(Min, Max);

        public FloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}