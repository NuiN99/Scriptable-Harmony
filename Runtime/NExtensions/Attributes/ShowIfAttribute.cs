using UnityEditor;
using UnityEngine;

namespace NuiN.NExtensions
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public readonly string varName;
        public readonly bool condition;

        public ShowIfAttribute(string varName, bool condition)
        {
            this.varName = varName;
            this.condition = condition;
        }

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(ShowIfAttribute))]
        internal class ShowIfDrawer : PropertyDrawer
        {
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                if (attribute is not ShowIfAttribute showIfAttribute) return EditorGUI.GetPropertyHeight(property, label, true);

                SerializedProperty varProperty = property.serializedObject.FindProperty(showIfAttribute.varName);
                bool conditionMet = IsConditionMet(varProperty, showIfAttribute.condition);
                return conditionMet ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                if (attribute is not ShowIfAttribute showIfAttribute)
                {
                    EditorGUI.PropertyField(position, property, label, true);
                    return;
                }
                
                SerializedProperty varProperty = property.serializedObject.FindProperty(showIfAttribute.varName);
                bool conditionMet = IsConditionMet(varProperty, showIfAttribute.condition);

                if (conditionMet)
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }

            static bool IsConditionMet(SerializedProperty property, bool condition)
            {
                return property != null && EvaluateCondition(property.boolValue, condition);
            }

            static bool EvaluateCondition(bool value, bool condition)
            {
                switch (value)
                {
                    case true when condition:
                    case false when !condition: return true;
                    default: return false;
                }
            }
        }
#endif
    }
}