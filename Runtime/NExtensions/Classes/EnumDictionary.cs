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
                if (index >= 0) values[index] = value;
                else { keys.Add(key); values.Add(value); }
            }
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EnumDictionary<,>), true)]
    public class EnumDictionaryPropertyDrawer : PropertyDrawer
    {
        const float INDENT_PER_LEVEL = 15f;
        const float SPLIT_KEY_WIDTH_RATIO = 0.25f;

        // Session caches
        static readonly Dictionary<Type, EnumCache> SEnumCache = new();
        static readonly Dictionary<string, bool> SHeaderExpanded = new();
        static readonly Dictionary<string, float[]> SRowHeights = new(); // per property key -> per-index flattened heights
        static readonly GUIContent SNone = GUIContent.none;

        struct EnumCache
        {
            public int[] values;     // int cast of enum values
            public string[] labels;  // ToString cached (or nicified)
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            string dictKey = GetDictKey(property);
            bool dictExpanded = GetExpanded(dictKey, defaultValue: false);

            var dictHeader = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            using (new EditorGUI.IndentLevelScope(0))
            {
                if (GUI.Button(dictHeader, label.text, EditorStyles.helpBox))
                {
                    dictExpanded = !dictExpanded;
                    SetExpanded(dictKey, dictExpanded);
                    // No immediate repaint-costly EditorPrefs call
                }
            }

            if (!dictExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            var enumType = GetEnumType();
            if (enumType == null)
            {
                EditorGUI.EndProperty();
                return;
            }
            EnumCache enumCache = GetOrBuildEnumCache(enumType);

            var keysProperty = property.FindPropertyRelative("keys");
            var valuesProperty = property.FindPropertyRelative("values");
            if (keysProperty == null || valuesProperty == null)
            {
                EditorGUI.EndProperty();
                return;
            }

            // Sync keys once per repaint cheaply by count check
            if (keysProperty.arraySize != enumCache.values.Length)
                SyncEnumWithKeys(keysProperty, valuesProperty, enumCache.values);

            float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            int indentLevel = EditorGUI.indentLevel + 1;
            float indentOffset = indentLevel * INDENT_PER_LEVEL;

            // Ensure heights cache exists and sized
            float[] rowHeights = GetOrResizeRowHeights(dictKey, valuesProperty.arraySize);

            for (int i = 0; i < keysProperty.arraySize; i++)
            {
                var valueProp = valuesProperty.GetArrayElementAtIndex(i);

                // Use cached flattened height; compute lazily when zero
                float valueHeight = rowHeights[i];
                if (Mathf.Approximately(valueHeight, 0f))
                {
                    valueHeight = GetFlattenedHeight(valueProp);
                    rowHeights[i] = valueHeight;
                }
                bool multiline = valueHeight > EditorGUIUtility.singleLineHeight * 1.1f;

                if (multiline)
                {
                    string rowKey = dictKey + "_row_" + i;
                    bool rowExpanded = GetExpanded(rowKey, defaultValue: false);

                    var rowHeader = new Rect(position.x + indentOffset, y, position.width - indentOffset, EditorGUIUtility.singleLineHeight);

                    if (rowExpanded)
                    {
                        float gap = EditorGUIUtility.standardVerticalSpacing;
                        float combinedH = EditorGUIUtility.singleLineHeight + gap + valueHeight + EditorGUIUtility.standardVerticalSpacing;

                        var combinedRect = new Rect(rowHeader.x, rowHeader.y, rowHeader.width, combinedH);
                        GUI.Box(combinedRect, SNone, EditorStyles.helpBox);

                        // Header label only, over same background
                        var headerLabel = new Rect(rowHeader.x + 6f, rowHeader.y + 2f, rowHeader.width - 12f, rowHeader.height);
                        if (GUI.Button(headerLabel, enumCache.labels[i], GUIStyle.none))
                        {
                            SetExpanded(rowKey, false);
                        }

                        // Value content
                        var valueRect = new Rect(rowHeader.x + 6f, rowHeader.y + EditorGUIUtility.singleLineHeight + gap, rowHeader.width - 12f, valueHeight);
                        DrawFlattened(valueRect, valueProp);

                        y += combinedH;
                    }
                    else
                    {
                        if (GUI.Button(rowHeader, enumCache.labels[i], EditorStyles.helpBox))
                        {
                            SetExpanded(rowKey, true);
                        }
                        y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    }
                }
                else
                {
                    float keyWidth = position.width * SPLIT_KEY_WIDTH_RATIO;
                    float valueWidth = position.width - keyWidth;

                    var keyRect = new Rect(position.x + indentOffset, y, keyWidth - indentOffset, EditorGUIUtility.singleLineHeight);
                    var valRect = new Rect(position.x + indentOffset + keyWidth, y, valueWidth, EditorGUIUtility.singleLineHeight);

                    EditorGUI.LabelField(keyRect, enumCache.labels[i]);
                    EditorGUI.PropertyField(valRect, valueProp, SNone, true);

                    y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            string dictKey = GetDictKey(property);
            bool dictExpanded = GetExpanded(dictKey, defaultValue: false);

            float total = EditorGUIUtility.singleLineHeight;
            if (!dictExpanded)
                return total;

            total += EditorGUIUtility.standardVerticalSpacing;

            var valuesProperty = property.FindPropertyRelative("values");
            if (valuesProperty == null)
                return total;

            float[] rowHeights = GetOrResizeRowHeights(dictKey, valuesProperty.arraySize);

            for (int i = 0; i < valuesProperty.arraySize; i++)
            {
                float vHeight = rowHeights[i];
                if (Mathf.Approximately(vHeight, 0f))
                {
                    vHeight = GetFlattenedHeight(valuesProperty.GetArrayElementAtIndex(i));
                    rowHeights[i] = vHeight;
                }
                bool multiline = vHeight > EditorGUIUtility.singleLineHeight * 1.1f;

                if (multiline)
                {
                    total += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    string rowKey = dictKey + "_row_" + i;
                    if (GetExpanded(rowKey, defaultValue: false))
                        total += vHeight + EditorGUIUtility.standardVerticalSpacing;
                }
                else
                {
                    total += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return total;
        }

        // Cached enum info
        Type GetEnumType()
        {
            var args = fieldInfo.FieldType.GetGenericArguments();
            if (args.Length >= 1 && args[0].IsEnum) return args[0];
            return null;
        }

        EnumCache GetOrBuildEnumCache(Type enumType)
        {
            if (!SEnumCache.TryGetValue(enumType, out var cache))
            {
                Array valuesArr = Enum.GetValues(enumType);
                int len = valuesArr.Length;
                var vals = new int[len];
                var labels = new string[len];
                for (int i = 0; i < len; i++)
                {
                    // Cast once, cache once
                    vals[i] = (int)Convert.ChangeType(valuesArr.GetValue(i), typeof(int));
                    labels[i] = ObjectNames.NicifyVariableName(valuesArr.GetValue(i).ToString());
                }
                cache = new EnumCache { values = vals, labels = labels };
                SEnumCache[enumType] = cache;
            }
            return cache;
        }

        // Expansion state (session cache, optionally sync to EditorPrefs on change)
        static bool GetExpanded(string key, bool defaultValue)
        {
            if (SHeaderExpanded.TryGetValue(key, out bool v)) return v;
            // Lazy load once from EditorPrefs (avoid hitting every frame)
            v = EditorPrefs.GetBool(key, defaultValue);
            SHeaderExpanded[key] = v;
            return v;
        }
        static void SetExpanded(string key, bool value)
        {
            if (!SHeaderExpanded.TryGetValue(key, out var cur) || cur != value)
            {
                SHeaderExpanded[key] = value;
                EditorPrefs.SetBool(key, value); // write only on change
            }
        }

        static float[] GetOrResizeRowHeights(string dictKey, int count)
        {
            if (!SRowHeights.TryGetValue(dictKey, out var arr) || arr == null || arr.Length != count)
            {
                arr = new float[count]; // zeros => lazy compute
                SRowHeights[dictKey] = arr;
            }
            return arr;
        }

        // Flattened draw and height (unchanged but non-alloc)
        static void DrawFlattened(Rect rect, SerializedProperty prop)
        {
            if (prop == null) return;

            if (prop.propertyType != SerializedPropertyType.Generic)
            {
                EditorGUI.PropertyField(rect, prop, SNone, true);
                return;
            }

            GUI.Box(rect, SNone, EditorStyles.helpBox);

            var iter = prop.Copy();
            var end = iter.GetEndProperty();

            float x = rect.x + 6f;
            float y = rect.y + 4f;
            float w = rect.width - 12f;

            bool enter = true;
            while (iter.NextVisible(enter) && !SerializedProperty.EqualContents(iter, end))
            {
                enter = false;
                float h = EditorGUI.GetPropertyHeight(iter, SNone, false);
                EditorGUI.PropertyField(new Rect(x, y, w, h), iter, false);
                y += h + EditorGUIUtility.standardVerticalSpacing;
            }
        }

        static float GetFlattenedHeight(SerializedProperty prop)
        {
            if (prop == null) return EditorGUIUtility.singleLineHeight;

            if (prop.propertyType != SerializedPropertyType.Generic)
                return EditorGUI.GetPropertyHeight(prop, SNone, true);

            float total = 4f;
            var iter = prop.Copy();
            var end = iter.GetEndProperty();
            bool enter = true;
            while (iter.NextVisible(enter) && !SerializedProperty.EqualContents(iter, end))
            {
                enter = false;
                total += EditorGUI.GetPropertyHeight(iter, SNone, false)
                      + EditorGUIUtility.standardVerticalSpacing;
            }
            total += 4f;
            return total + 2f;
        }

        static void SyncEnumWithKeys(SerializedProperty keysProperty, SerializedProperty valuesProperty, int[] enumInts)
        {
            // Only rewrite if mismatch; avoid ClearArray (causes allocations and lost references)
            if (keysProperty.arraySize != enumInts.Length)
                keysProperty.arraySize = enumInts.Length;

            for (int i = 0; i < enumInts.Length; i++)
            {
                var elem = keysProperty.GetArrayElementAtIndex(i);
                if (elem.intValue != enumInts[i]) elem.intValue = enumInts[i];
            }

            // Keep values array sized, but do not Reset() which can be expensive; Unity initializes defaults
            if (valuesProperty.arraySize > enumInts.Length)
                valuesProperty.arraySize = enumInts.Length;
            else if (valuesProperty.arraySize < enumInts.Length)
                valuesProperty.arraySize = enumInts.Length;
        }

        static string GetDictKey(SerializedProperty property)
        {
            return property.serializedObject.targetObject.GetInstanceID() + "_" + property.propertyPath;
        }
    }
#endif
}
