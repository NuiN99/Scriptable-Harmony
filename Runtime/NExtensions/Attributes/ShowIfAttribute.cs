using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NuiN.NExtensions
{
    //https://github.com/SebLague/Marching-Cubes/blob/master/Assets/Editor/ConditionalHidePropertyDrawer.cs
    public class ShowIfAttribute : PropertyAttribute
    {
        public readonly string conditionalSourceField;
        public readonly bool showIfTrue;
        public readonly int enumIndex;

        public ShowIfAttribute(string boolVariableName, bool showIfTrue)
        {
            conditionalSourceField = boolVariableName;
            this.showIfTrue = showIfTrue;
        }

        public ShowIfAttribute(string enumVariableName, int enumIndex)
        {
            conditionalSourceField = enumVariableName;
            this.enumIndex = enumIndex;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ShowIfAttribute), true)]
    internal class ConditionalHidePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute attr = (ShowIfAttribute)attribute;
            
            bool enabled = GetConditionalHideAttributeResult(attr, property);        

            if (enabled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }        
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute attr = (ShowIfAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(attr, property);

            if (enabled)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            
            return -EditorGUIUtility.standardVerticalSpacing;
        }

        static bool GetConditionalHideAttributeResult(ShowIfAttribute attr, SerializedProperty property)
        {
            string propertyPath = property.propertyPath;
            string conditionPath = propertyPath.Replace(property.name, attr.conditionalSourceField);
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath) ?? property.serializedObject.FindProperty(attr.conditionalSourceField);

            if (sourcePropertyValue == null)
            {
                object targetObject = property.serializedObject.targetObject;
                Type targetType = targetObject.GetType();
        
                FieldInfo field = targetType.GetField(attr.conditionalSourceField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                if (field != null)
                {
                    object fieldValue = field.GetValue(targetObject);
                    return CheckFieldValue(attr, fieldValue);
                }

                PropertyInfo propertyInfo = targetType.GetProperty(attr.conditionalSourceField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                if (propertyInfo != null)
                {
                    object propValue = propertyInfo.GetValue(targetObject);
                    return CheckFieldValue(attr, propValue);
                }

                Debug.LogError($"Field or property '{attr.conditionalSourceField}' not found on object of type '{targetType}'.");
                return true;
            }

            return CheckPropertyType(attr, sourcePropertyValue);
        }

        static bool CheckFieldValue(ShowIfAttribute attr, object fieldValue)
        {
            if (fieldValue is bool boolValue)
            {
                return boolValue == attr.showIfTrue;
            }

            if (fieldValue is Enum enumValue)
            {
                return Convert.ToInt32(enumValue) == attr.enumIndex;
            }

            Debug.LogError($"Unsupported data type '{fieldValue?.GetType()}' for conditional hiding.");
            return true;
        }

        static bool CheckPropertyType(ShowIfAttribute attr, SerializedProperty sourcePropertyValue)
        {
            switch (sourcePropertyValue.propertyType)
            {                
                case SerializedPropertyType.Boolean:
                    return sourcePropertyValue.boolValue;                
                case SerializedPropertyType.Enum:
                    return sourcePropertyValue.enumValueIndex == attr.enumIndex;
                default:
                    Debug.LogError("Data type of the property used for conditional hiding [" + sourcePropertyValue.propertyType + "] is currently not supported");
                    return true;
            }
        }
    }
#endif
}