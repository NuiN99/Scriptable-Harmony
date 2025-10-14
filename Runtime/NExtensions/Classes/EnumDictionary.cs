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
        const float INDENT_PER_LEVEL = 15f;
        const float SPLIT_KEY_WIDTH_RATIO = 0.25f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Persisted expansion for the whole dictionary (no Foldout used)
            string dictKey = $"{property.serializedObject.targetObject.GetInstanceID()}_{property.propertyPath}";
            bool dictExpanded = EditorPrefs.GetBool(dictKey, false);

            // Draw dictionary header as a full-width button (no arrow)
            var dictHeader = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            using (new EditorGUI.IndentLevelScope(0))
            {
                if (GUI.Button(dictHeader, label.text, EditorStyles.helpBox))
                {
                    dictExpanded = !dictExpanded;
                    EditorPrefs.SetBool(dictKey, dictExpanded);
                }
            }

            if (!dictExpanded)
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

            // Resolve generic arguments
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

            // Keep keys synced to enum members
            var enumValues = Enum.GetValues(enumType);
            SyncEnumWithKeys(keysProperty, valuesProperty, enumValues);

            float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            float indentOffset = EditorGUI.indentLevel * INDENT_PER_LEVEL;

            for (int i = 0; i < keysProperty.arraySize; i++)
            {
                var valueProp = valuesProperty.GetArrayElementAtIndex(i);
                // Compute expanded content height for this value, using flattened drawing rules
                float valueHeight = GetFlattenedHeight(valueProp);
                bool multiline = valueHeight > EditorGUIUtility.singleLineHeight * 1.1f;

                Enum enumValue = (Enum)Enum.ToObject(enumType, keysProperty.GetArrayElementAtIndex(i).intValue);

                if (multiline)
                {
                    string rowKey = $"{dictKey}_row_{i}";
                    bool rowExpanded = EditorPrefs.GetBool(rowKey, false);

                    // Header rect
                    var rowHeader = new Rect(position.x + indentOffset, y, position.width - indentOffset, EditorGUIUtility.singleLineHeight);

                    // If expanded, compute combined rect for header + value and draw one background
                    if (rowExpanded)
                    {
                        // Space below header before content
                        float gap = EditorGUIUtility.standardVerticalSpacing;

                        // Total height: header + gap + value + bottom gap
                        float combinedH = EditorGUIUtility.singleLineHeight + gap + valueHeight + EditorGUIUtility.standardVerticalSpacing;

                        // One continuous background
                        var combinedRect = new Rect(rowHeader.x, rowHeader.y, rowHeader.width, combinedH);
                        GUI.Box(combinedRect, GUIContent.none, EditorStyles.helpBox); // Cohesive single box [web:39]

                        // Draw header as a label-like button on top of the same background
                        var headerLabel = new Rect(rowHeader.x + 6f, rowHeader.y + 2f, rowHeader.width - 12f, rowHeader.height);
                        if (GUI.Button(headerLabel, enumValue.ToString(), GUIStyle.none)) // no separate box so it blends [web:19]
                        {
                            EditorPrefs.SetBool(rowKey, false);
                        }

                        // Draw value content inside the same box, with padding
                        var valueRect = new Rect(
                            rowHeader.x + 6f,
                            rowHeader.y + EditorGUIUtility.singleLineHeight + gap,
                            rowHeader.width - 12f,
                            valueHeight
                        );
                        DrawFlattened(valueRect, valueProp);

                        // Advance Y by the combined area
                        y += combinedH;
                    }
                    else
                    {
                        // Collapsed state: draw a single header box only
                        if (GUI.Button(rowHeader, enumValue.ToString(), EditorStyles.helpBox)) // header-only box [web:39]
                        {
                            rowExpanded = !rowExpanded;
                            EditorPrefs.SetBool(rowKey, rowExpanded);
                        }

                        y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    }
                }
                else
                {
                    // Single-line row: key label left, value right
                    float keyWidth = position.width * SPLIT_KEY_WIDTH_RATIO;
                    float valueWidth = position.width - keyWidth;

                    var keyRect = new Rect(position.x + indentOffset, y, keyWidth - indentOffset, EditorGUIUtility.singleLineHeight);
                    var valRect = new Rect(position.x + indentOffset + keyWidth, y, valueWidth, EditorGUIUtility.singleLineHeight);

                    EditorGUI.LabelField(keyRect, enumValue.ToString());
                    EditorGUI.PropertyField(valRect, valueProp, GUIContent.none, true);

                    y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Same key used in OnGUI
            string dictKey = $"{property.serializedObject.targetObject.GetInstanceID()}_{property.propertyPath}";
            bool dictExpanded = EditorPrefs.GetBool(dictKey, false);

            // Header always occupies one line
            float total = EditorGUIUtility.singleLineHeight;

            if (!dictExpanded)
                return total;

            total += EditorGUIUtility.standardVerticalSpacing;

            var keysProperty = property.FindPropertyRelative("keys");
            var valuesProperty = property.FindPropertyRelative("values");
            if (keysProperty == null || valuesProperty == null)
                return total;

            for (int i = 0; i < valuesProperty.arraySize; i++)
            {
                var valueProp = valuesProperty.GetArrayElementAtIndex(i);

                // Height used for layout logic (flattened)
                float vHeight = GetFlattenedHeight(valueProp);
                bool multiline = vHeight > EditorGUIUtility.singleLineHeight * 1.1f;

                if (multiline)
                {
                    // Row header (full-width clickable)
                    total += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    // Use the SAME per-row expansion key as OnGUI
                    string rowKey = $"{dictKey}_row_{i}";
                    bool rowExpanded = EditorPrefs.GetBool(rowKey, false);
                    if (rowExpanded)
                    {
                        total += vHeight + EditorGUIUtility.standardVerticalSpacing;
                    }
                }
                else
                {
                    // Single-line row
                    total += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return total;
        }
        
        // Draw complex properties flattened (no nested foldouts)
        static void DrawFlattened(Rect rect, SerializedProperty prop)
        {
            if (prop == null)
                return;

            // Primitive or non-generic: draw as-is
            if (prop.propertyType != SerializedPropertyType.Generic)
            {
                EditorGUI.PropertyField(rect, prop, GUIContent.none, true);
                return;
            }

            // Box background
            GUI.Box(rect, GUIContent.none, EditorStyles.helpBox);

            // Iterate children without drawing the root as a foldout
            var iter = prop.Copy();
            var end = iter.GetEndProperty();

            float x = rect.x + 6f;
            float y = rect.y + 4f;
            float w = rect.width - 12f;

            bool enterChildren = true;
            while (iter.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iter, end))
            {
                enterChildren = false;
                float h = EditorGUI.GetPropertyHeight(iter, GUIContent.none, false);
                var r = new Rect(x, y, w, h);
                EditorGUI.PropertyField(r, iter, false); // includeChildren: false prevents nested foldouts
                y += h + EditorGUIUtility.standardVerticalSpacing;
            }
        }
        
        // Height calculation matching DrawFlattened
        static float GetFlattenedHeight(SerializedProperty prop)
        {
            if (prop == null)
                return EditorGUIUtility.singleLineHeight;

            if (prop.propertyType != SerializedPropertyType.Generic)
                return EditorGUI.GetPropertyHeight(prop, GUIContent.none, true);

            float total = 4f; // top padding inside box

            var iter = prop.Copy();
            var end = iter.GetEndProperty();
            bool enterChildren = true;
            while (iter.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iter, end))
            {
                enterChildren = false;
                total += EditorGUI.GetPropertyHeight(iter, GUIContent.none, false)
                         + EditorGUIUtility.standardVerticalSpacing;
            }

            total += 4f; // bottom padding
            // Add helpBox chrome
            return total + 2f; // slight extra for border
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