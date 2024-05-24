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
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    internal class ConditionalHidePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute attr = (ShowIfAttribute)attribute;
            
            bool enabled = GetConditionalHideAttributeResult(attr, property) == attr.showIfTrue;        

            if (enabled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }        
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute attr = (ShowIfAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(attr, property) == attr.showIfTrue;

            if (enabled)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            
            return -EditorGUIUtility.standardVerticalSpacing;
        }

        static bool GetConditionalHideAttributeResult(ShowIfAttribute attr, SerializedProperty property)
        {
            SerializedProperty sourcePropertyValue;

            if (!property.isArray)
            {
                string propertyPath = property.propertyPath;
                string conditionPath = propertyPath.Replace(property.name, attr.conditionalSourceField);
                sourcePropertyValue = property.serializedObject.FindProperty(conditionPath) ?? property.serializedObject.FindProperty(attr.conditionalSourceField);
            }
            else
            {
                sourcePropertyValue = property.serializedObject.FindProperty(attr.conditionalSourceField);
            }
            
            return sourcePropertyValue == null || CheckPropertyType(attr,sourcePropertyValue);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        static bool CheckPropertyType(ShowIfAttribute attr, SerializedProperty sourcePropertyValue)
        {
            switch (sourcePropertyValue.propertyType)
            {                
                case SerializedPropertyType.Boolean:
                    return sourcePropertyValue.boolValue;                
                case SerializedPropertyType.Enum:
                    return sourcePropertyValue.enumValueIndex != attr.enumIndex;
                default:
                    Debug.LogError("Data type of the property used for conditional hiding [" + sourcePropertyValue.propertyType + "] is currently not supported");
                    return true;
            }
        }
    }
#endif
}