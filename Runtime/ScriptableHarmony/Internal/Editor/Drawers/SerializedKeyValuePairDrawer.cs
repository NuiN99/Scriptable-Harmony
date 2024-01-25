#if UNITY_EDITOR

using NuiN.ScriptableHarmony.Core;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializedKeyValuePair<,>))]
public class SerializedKeyValuePairDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Get the key and value properties
        SerializedProperty keyProperty = property.FindPropertyRelative("key");
        SerializedProperty valueProperty = property.FindPropertyRelative("value");

        // Create GUIContent instances for key and value labels
        GUIContent keyLabel = new GUIContent("Key");
        GUIContent valueLabel = new GUIContent("Value");

        // Calculate label width for both key and value
        float labelWidth = Mathf.Max(EditorGUIUtility.labelWidth, EditorStyles.label.CalcSize(keyLabel).x, EditorStyles.label.CalcSize(valueLabel).x);

        // Draw the key label and property on the left half
        Rect keyLabelRect = new Rect(position.x, position.y, labelWidth, position.height);
        EditorGUI.LabelField(keyLabelRect, keyLabel);
        Rect keyRect = new Rect(position.x - 20 + labelWidth, position.y, position.width * 0.5f - labelWidth, position.height);
        EditorGUI.PropertyField(keyRect, keyProperty, GUIContent.none, true);

        // Draw the value label and property on the right half
        Rect valueLabelRect = new Rect(position.x + position.width * 0.5f, position.y, labelWidth, position.height);
        EditorGUI.LabelField(valueLabelRect, valueLabel);
        Rect valueRect = new Rect(position.x + position.width * 0.5f + labelWidth, position.y, position.width * 0.5f - labelWidth, position.height);
        EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none, true);
    }
}

#endif