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
        bool _isExpanded;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw foldout
            _isExpanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                _isExpanded,
                label,
                true
            );

            if (!_isExpanded)
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

            // Draw each key-value pair side by side
            float currentY = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            for (int i = 0; i < keysProperty.arraySize; i++)
            {
                Rect rowRect = new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight);

                // Split row into two parts (key and value)
                float keyWidth = position.width * 0.25f;
                float valueWidth = position.width * 0.75f;

                Rect keyRect = new Rect(rowRect.x, rowRect.y, keyWidth, rowRect.height);
                Rect valueRect = new Rect(rowRect.x + keyWidth, rowRect.y, valueWidth, rowRect.height);

                // Draw key (enum dropdown, non-editable)
                GUI.enabled = false; // Disable editing of keys
                Enum enumValue = (Enum)Enum.ToObject(enumType, keysProperty.GetArrayElementAtIndex(i).intValue);
                EditorGUI.EnumPopup(keyRect, enumValue);
                GUI.enabled = true;

                // Draw value
                var valueProp = valuesProperty.GetArrayElementAtIndex(i);
                if (valueProp != null)
                {
                    EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none, true);
                }

                currentY += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!_isExpanded) return EditorGUIUtility.singleLineHeight;

            var keysProperty = property.FindPropertyRelative("keys");
            var valuesProperty = property.FindPropertyRelative("values");

            if (keysProperty == null || valuesProperty == null) return EditorGUIUtility.singleLineHeight;

            float totalHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // For the foldout

            // Account for each row (key-value pair)
            totalHeight += keysProperty.arraySize * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

            return totalHeight;
        }

        void SyncEnumWithKeys(SerializedProperty keysProperty, SerializedProperty valuesProperty, Array enumValues)
        {
            // Clear the keys property
            keysProperty.ClearArray();

            // Regenerate keys from enum values
            foreach (Enum enumValue in enumValues)
            {
                int enumInt = Convert.ToInt32(enumValue);
                keysProperty.InsertArrayElementAtIndex(keysProperty.arraySize);
                keysProperty.GetArrayElementAtIndex(keysProperty.arraySize - 1).intValue = enumInt;
            }

            // Trim or extend the values property to match the size of the keys
            while (valuesProperty.arraySize > keysProperty.arraySize)
            {
                valuesProperty.DeleteArrayElementAtIndex(valuesProperty.arraySize - 1);
            }

            while (valuesProperty.arraySize < keysProperty.arraySize)
            {
                valuesProperty.InsertArrayElementAtIndex(valuesProperty.arraySize);
                valuesProperty.GetArrayElementAtIndex(valuesProperty.arraySize - 1).Reset();
            }
        }
    }
#endif
}