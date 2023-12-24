using System;
using System.Reflection;
using NuiN.ScriptableHarmony.Core.Editor.Tools;
using NuiN.ScriptableHarmony.Internal.Helpers;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Core.Editor.Helpers
{
#if UNITY_EDITOR
    internal static class SOReferenceGUIHelper
    {
        static readonly Color GetterColor = new(0.7f, 0.9f, 0.95f, 1f);
        static readonly Color SetterColor = new(0.9f, 0.7f, 0.7f, 1f);
        static readonly Color OverlayColor = new(0.1f, 0.1f, 0.1f, 1);

        public static float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => EditorGUIUtility.singleLineHeight;

        public static void VarRefGUI(SOType soType, Access accessType, string propertyName, Rect position, SerializedProperty property, GUIContent label, FieldInfo fieldInfo)
        {
            Color color = accessType switch { Access.Getter => GetterColor, Access.Setter => SetterColor, _ => Color.white };
            
            EditorGUI.BeginProperty(position, label, property);
            
            SerializedProperty variableProperty = property.FindPropertyRelative(propertyName);

            Type variableType = fieldInfo.FieldType.GetGenericArguments()[0];
            string typeName = GetReadableTypeName(variableType);

            switch (soType)
            {
                case SOType.RuntimeSet: typeName += "RuntimeSetSO"; break;
                case SOType.RuntimeSingle: typeName += "RuntimeSingleSO"; break;
                case SOType.Variable:
                {
                    typeName = $"{typeName}VariableSO";
                    break;
                }
                case SOType.ListVariable:
                {
                    typeName = $"{typeName}ListVariableSO";
                    break;
                }
                case SOType.DictionaryVariable:
                {
                    Type[] genericArguments = fieldInfo.FieldType.GetGenericArguments();
                    string type1 = GetReadableTypeName(genericArguments[0]);
                    string type2 = GetReadableTypeName(genericArguments[1]);
                    typeName = $"{type1}{type2}DictionaryVariableSO";
                    break;
                }
            }
            
            if (variableProperty.objectReferenceValue == null)
            {
                DrawLabel();
                DrawFindButton();
                DrawPropertyField();
                DrawHelpBox();
            }
            else
            {
                DrawPropertyField();
                DrawFindButton();
            }

            EditorGUI.EndProperty();
            return;

            void DrawFindButton()
            {
                Rect buttonPosition = new Rect(position.x + position.width - EditorGUIUtility.singleLineHeight, position.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
                GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
                GUIContent buttonText = new GUIContent("^");
                Color originalColor = GUI.backgroundColor;
                GUI.backgroundColor = new Color(0.4f,0.4f,0.4f, 1);
                if (GUI.Button(buttonPosition, buttonText, buttonStyle))
                {
                    FindScriptableObjectWindow.OpenFindWindow(typeName, variableProperty);
                }
                GUI.backgroundColor = originalColor;
            }

            void DrawHelpBox()
            {
                position.y += EditorGUIUtility.singleLineHeight;
                float labelWidth = EditorGUIUtility.labelWidth + 2.5f;
                float helpBoxWidth = position.width - labelWidth - EditorGUIUtility.singleLineHeight; // Adjust width to make room for the button
                Rect helpBoxPosition = new Rect(position.x + labelWidth, position.y - EditorGUIUtility.singleLineHeight, helpBoxWidth, EditorGUIUtility.singleLineHeight);
                EditorGUI.DrawRect(helpBoxPosition, OverlayColor);
                EditorGUI.HelpBox(helpBoxPosition, $"None ({typeName})", MessageType.Warning);
            }

            void DrawLabel()
            {
                EditorStyles.label.normal.textColor = color;
                Rect labelPosition = new Rect(position.x, position.y, position.width - EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(labelPosition, label);
                EditorStyles.label.normal.textColor = Color.white;
            }

            void DrawPropertyField()
            {
                Rect objectFieldPosition = new Rect(position.x, position.y, position.width - EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
                EditorGUI.BeginProperty(objectFieldPosition, GUIContent.none, variableProperty);
                EditorStyles.label.normal.textColor = color;
                EditorGUI.PropertyField(objectFieldPosition, variableProperty, label, true);
                EditorStyles.label.normal.textColor = Color.white;
                EditorGUI.EndProperty();
            }
        }
        
        static string GetReadableTypeName(Type type)
        {
            return type switch
            {
                not null when type == typeof(float) => "Float",
                not null when type == typeof(bool) => "Bool",
                not null when type == typeof(int) => "Int",
                not null when type == typeof(long) => "Long",
                _ => type?.Name
            };
        }
    }
#endif
}
