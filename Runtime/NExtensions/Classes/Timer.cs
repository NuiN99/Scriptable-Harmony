using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEditor;

namespace NuiN.NExtensions
{
    [Serializable]
    public class Timer
    {
        float _timeStart;

        [SerializeField, Tooltip("Time range before the timer completes")]
        Vector2 durationRange;

        [SerializeField, Tooltip("How long before the timer completes")]
        float duration;
        
        [SerializeField, Tooltip("Automatically set random duration upon completion")]
        bool random;

        [SerializeField, Tooltip("Makes the timer return completed on the first call")]
        bool startCompleted = true;

        public Timer InitRandom()
        {
            duration = Random.Range(durationRange.x, durationRange.y);
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

            if (random) InitRandom();
            return true;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Timer))]
    public class TimerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                                                    property.isExpanded, label, true);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                float lineHeight = EditorGUIUtility.singleLineHeight;
                float indentation = EditorGUI.indentLevel * 15f;

                EditorGUI.PropertyField(new Rect(position.x, position.y + lineHeight, position.width, lineHeight), property.FindPropertyRelative("startCompleted"));

                if (property.FindPropertyRelative("random").boolValue)
                {
                    float minMaxWidth = position.width * 0.5f;
                    Rect minRect = new Rect(position.x, position.y + 4 * lineHeight, minMaxWidth, lineHeight);
                    Rect maxRect = new Rect(position.x + minMaxWidth, position.y + 4 * lineHeight, minMaxWidth, lineHeight);

                    EditorGUI.LabelField(minRect, new GUIContent("Min"), EditorStyles.boldLabel);
                    EditorGUI.LabelField(maxRect, new GUIContent("Max"), EditorStyles.boldLabel);

                    minRect.y += lineHeight;
                    maxRect.y += lineHeight;

                    EditorGUI.PropertyField(minRect, property.FindPropertyRelative("durationRange").FindPropertyRelative("x"), GUIContent.none);
                    EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("durationRange").FindPropertyRelative("y"), GUIContent.none);
                }
                else
                {
                    EditorGUI.LabelField(new Rect(position.x, position.y + 4 * lineHeight, position.width, lineHeight), new GUIContent("Duration"), EditorStyles.boldLabel);
                    EditorGUI.PropertyField(new Rect(position.x, position.y + 5 * lineHeight, position.width, lineHeight), property.FindPropertyRelative("duration"), GUIContent.none);
                }
                
                EditorGUI.PropertyField(new Rect(position.x, position.y + 3 * lineHeight, position.width, lineHeight), property.FindPropertyRelative("random"));

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded)
            {
                height += 5 * EditorGUI.GetPropertyHeight(property.FindPropertyRelative("startCompleted"));
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("duration"));
            }

            return height;
        }
    }
#endif
}
