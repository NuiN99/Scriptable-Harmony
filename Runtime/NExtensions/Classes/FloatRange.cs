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
        public float Clamp(float value) => Mathf.Clamp(value, Min, Max);

        public FloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }

    [Serializable]
    public class IntRange
    {
        [field: SerializeField] public int Min { get; private set; }
        [field: SerializeField] public int Max { get; private set; }

        public int Random() => UnityEngine.Random.Range(Min, Max + 1);
        public int Clamp(int value) => Math.Clamp(value, Min, Max);

        public IntRange(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}