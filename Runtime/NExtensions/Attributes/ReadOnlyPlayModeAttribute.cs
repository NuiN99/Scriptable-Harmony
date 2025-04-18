using UnityEditor;
using UnityEngine;

namespace NuiN.NExtensions
{
    public class ReadOnlyPlayModeAttribute : PropertyAttribute { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ReadOnlyPlayModeAttribute))]
    internal class ReadOnlyPlayModeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return Application.isPlaying ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if (Application.isPlaying)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = true;
            }

            EditorGUI.indentLevel = indent;
        }
    }
#endif

    public class HideInPlayModeAttribute : PropertyAttribute { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(HideInPlayModeAttribute))]
    internal class HideInPlayModeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return !Application.isPlaying ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if (!Application.isPlaying)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            EditorGUI.indentLevel = indent;
        }
    }
#endif
}