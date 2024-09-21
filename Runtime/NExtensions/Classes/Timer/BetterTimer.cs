using System;
using UnityEditor;
using UnityEngine;

namespace NuiN.NExtensions
{
    [Serializable]
    public class BetterTimer
    {
        [field: SerializeField] public float Duration { get; private set; }
        public float Time { get; private set; }
        public float Lerp => Time / Duration;

        float _startTime;

        public bool IsComplete
        {
            get
            {
                float worldTime = UnityEngine.Time.time;
                Time = Mathf.Clamp(worldTime - _startTime, 0, Duration);
                return Time >= Duration;
            }
        }

        public BetterTimer(float duration)
        {
            Duration = duration;
            Restart();
        }

        public void Restart()
        {
            _startTime = UnityEngine.Time.time;
            Time = 0;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(BetterTimer))]
    public class BetterTimerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            // using [field: SerializeField] for properties changes their Get name when using reflection
            SerializedProperty durationProperty = property.FindPropertyRelative("<Duration>k__BackingField");
            EditorGUI.PropertyField(position, durationProperty, label);
            EditorGUI.EndProperty();
        }
    }
#endif
}