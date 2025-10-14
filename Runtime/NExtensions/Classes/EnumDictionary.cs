using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NuiN.NExtensions
{
    [Serializable]
    public class EnumDictionary<TEnum, TValue> where TEnum : Enum
    {
        [SerializeField] List<TEnum> keys = new();
        [SerializeField] List<TValue> values = new();

        public TValue this[TEnum key]
        {
            get
            {
                int index = keys.IndexOf(key);
                return index >= 0 ? values[index] : default;
            }
            set
            {
                int index = keys.IndexOf(key);
                if (index >= 0)
                {
                    values[index] = value;
                }
                else
                {
                    keys.Add(key);
                    values.Add(value);
                }
            }
        }
    }

    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EnumDictionary<,>), true)]
    public class EnumDictionaryPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            string key = $"{property.serializedObject.targetObject.GetInstanceID()}_{property.propertyPath}";
            bool isExpanded = EditorPrefs.GetBool(key, false);

            bool newExpanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                isExpanded,
                label,
                true
            );

            if (newExpanded != isExpanded)
            {
                EditorPrefs.SetBool(key, newExpanded);
                isExpanded = newExpanded;
            }

            if (!isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            EditorGUI.indentLevel++;

            var keysProperty = property.FindPropertyRelative("keys");
            var valuesProperty = property.FindPropertyRelative("values");

            if (keysProperty == null || valuesProperty == null)
            {
                EditorGUI.EndProperty();
                return;
            }

            Type[] genericArguments = fieldInfo.FieldType.GetGenericArguments();
            if (genericArguments.Length < 2)
            {
                EditorGUI.EndProperty();
                return;
            }

            var enumType = genericArguments[0];
            if (!enumType.IsEnum)
            {
                EditorGUI.EndProperty();
                return;
            }

            var enumValues = Enum.GetValues(enumType);
            SyncEnumWithKeys(keysProperty, valuesProperty, enumValues);

            float currentY = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            for (int i = 0; i < keysProperty.arraySize; i++)
            {
                var valueProp = valuesProperty.GetArrayElementAtIndex(i);
                float valueHeight = EditorGUI.GetPropertyHeight(valueProp, GUIContent.none, true);

                Rect rowRect = new Rect(position.x, currentY, position.width, valueHeight);

                float keyWidth = position.width * 0.25f;
                float valueWidth = position.width * 0.75f;

                Rect keyRect = new Rect(rowRect.x, rowRect.y, keyWidth, EditorGUIUtility.singleLineHeight);
                Rect valueRect = new Rect(rowRect.x + keyWidth, rowRect.y, valueWidth, valueHeight);

                GUI.enabled = false;
                Enum enumValue = (Enum)Enum.ToObject(enumType, keysProperty.GetArrayElementAtIndex(i).intValue);
                EditorGUI.EnumPopup(keyRect, enumValue);
                GUI.enabled = true;

                if (valueProp != null)
                    EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none, true);

                currentY += valueHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            string key = $"{property.serializedObject.targetObject.GetInstanceID()}_{property.propertyPath}";
            bool isExpanded = EditorPrefs.GetBool(key, false);

            if (!isExpanded)
                return EditorGUIUtility.singleLineHeight;

            var keysProperty = property.FindPropertyRelative("keys");
            var valuesProperty = property.FindPropertyRelative("values");

            float totalHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (keysProperty != null && valuesProperty != null)
            {
                for (int i = 0; i < valuesProperty.arraySize; i++)
                {
                    var valueProp = valuesProperty.GetArrayElementAtIndex(i);
                    totalHeight += EditorGUI.GetPropertyHeight(valueProp, GUIContent.none, true)
                                   + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return totalHeight;
        }

        void SyncEnumWithKeys(SerializedProperty keysProperty, SerializedProperty valuesProperty, Array enumValues)
        {
            keysProperty.ClearArray();

            foreach (Enum enumValue in enumValues)
            {
                int enumInt = Convert.ToInt32(enumValue);
                keysProperty.InsertArrayElementAtIndex(keysProperty.arraySize);
                keysProperty.GetArrayElementAtIndex(keysProperty.arraySize - 1).intValue = enumInt;
            }

            while (valuesProperty.arraySize > keysProperty.arraySize)
                valuesProperty.DeleteArrayElementAtIndex(valuesProperty.arraySize - 1);

            while (valuesProperty.arraySize < keysProperty.arraySize)
            {
                valuesProperty.InsertArrayElementAtIndex(valuesProperty.arraySize);
                valuesProperty.GetArrayElementAtIndex(valuesProperty.arraySize - 1).Reset();
            }
        }
    }
#endif
}