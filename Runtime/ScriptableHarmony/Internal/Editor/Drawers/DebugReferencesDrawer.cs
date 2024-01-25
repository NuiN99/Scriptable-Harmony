#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using NuiN.ScriptableHarmony.Core;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Editor
{
    [CustomPropertyDrawer(typeof(GetSetReferencesContainer))]
    internal class GetSetReferencesContainerDrawer : PropertyDrawer
    {
        string[] _propertyNames = { "Getters", "Setters", "getters", "setters" };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DebugReferencesDrawerBase.DebugReferenceGUI(_propertyNames.ToList(), "Script References - ", position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return DebugReferencesDrawerBase.GetPropertyHeight(_propertyNames.ToList(), property, label);
        }
    }
    
    [CustomPropertyDrawer(typeof(RuntimeObjectReferencesContainer))]
    internal class RuntimeObjectReferencesContainerDrawer : PropertyDrawer
    {
        string[] _propertyNames = { "prefabs", "scene" };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DebugReferencesDrawerBase.DebugReferenceGUI(_propertyNames.ToList(), "Component References - ", position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return DebugReferencesDrawerBase.GetPropertyHeight(_propertyNames.ToList(), property, label);
        }
    }

    internal static class DebugReferencesDrawerBase
    {
        public static void DebugReferenceGUI(List<string> propertyNames, string labelPrefix, Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            List<SerializedProperty> properties = propertyNames.Select(property.FindPropertyRelative).ToList();
            int totalReferencesCount = properties.Sum(prop => prop.arraySize);

            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, $"{labelPrefix}{totalReferencesCount}", true);

            if (Event.current.type == EventType.MouseDown && foldoutRect.Contains(Event.current.mousePosition))
            {
                property.isExpanded = !property.isExpanded;
                Event.current.Use();
            }

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                float lineHeight = EditorGUIUtility.singleLineHeight;
                float startY = position.y + lineHeight;

                int index = 0;
                foreach (var serializedProperty in properties)
                {
                    startY = DrawReadOnlyList(new Rect(position.x, startY, position.width, lineHeight), propertyNames[index], serializedProperty);
                    index++;
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public static float DrawReadOnlyList(Rect rect, string label, SerializedProperty list)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(rect, list, new GUIContent($"{label[0].ToString().ToUpper()}{label[1..]}"), true);
            EditorGUI.EndDisabledGroup();
            return rect.y + EditorGUI.GetPropertyHeight(list, true);
        }

        public static float GetPropertyHeight(List<string> propertyNames, SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;
            if (!property.isExpanded) return height;

            height += propertyNames.Sum(name => GetListHeight(property.FindPropertyRelative(name)));
            return height;
        }

        public static float GetListHeight(SerializedProperty list)
        {
            return EditorGUI.GetPropertyHeight(list, true);
        }
    }
}
#endif
