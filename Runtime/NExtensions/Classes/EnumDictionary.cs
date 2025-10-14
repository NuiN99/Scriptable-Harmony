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
        static readonly Dictionary<string, bool> SExpanded = new(); // holds both header and rows
        static readonly Dictionary<string, float[]> SRowHeights = new(); // per property key -> per-index flattened heights
        static readonly GUIContent SNone = GUIContent.none;

        struct EnumCache
        {
            public int[] values;     // int cast of enum values
            public string[] labels;  // Nicified names
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            string dictKey = GetDictKey(property);
            bool dictExpanded = GetExpanded(dictKey, defaultValue: false);

            // Dictionary header (no arrow). Use MouseUp so we don't steal focus on MouseDown.
            var dictHeader = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            GUI.Box(dictHeader, GUIContent.none, EditorStyles.helpBox);
            var dictLabel = new Rect(dictHeader.x + 6f, dictHeader.y + 2f, dictHeader.width - 12f, dictHeader.height);
            EditorGUI.LabelField(dictLabel, label);

            var e = Event.current;
            if (e.type == EventType.MouseUp && e.button == 0 && dictHeader.Contains(e.mousePosition))
            {
                dictExpanded = !dictExpanded;
                SetExpanded(dictKey, dictExpanded);
                e.Use();
                if (!dictExpanded)
                {
                    GUIUtility.keyboardControl = 0;
                    GUIUtility.hotControl = 0;
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

            // Sync keys quickly by size, no clear/reinsert churn
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

                        // One continuous background box
                        var combinedRect = new Rect(rowHeader.x, rowHeader.y, rowHeader.width, combinedH);
                        GUI.Box(combinedRect, GUIContent.none, EditorStyles.helpBox);

                        // Header text over same background; do not use Button (prevents focus grab)
                        var headerLabel = new Rect(rowHeader.x + 6f, rowHeader.y + 2f, rowHeader.width - 12f, rowHeader.height);
                        EditorGUI.LabelField(headerLabel, enumCache.labels[i]);

                        // Toggle collapse on MouseUp only
                        if (e.type == EventType.MouseUp && e.button == 0 && rowHeader.Contains(e.mousePosition))
                        {
                            SetExpanded(rowKey, false);
                            e.Use();
                            GUIUtility.keyboardControl = 0;
                            GUIUtility.hotControl = 0;
                        }

                        // Value content inside the same box
                        var valueRect = new Rect(rowHeader.x + 6f, rowHeader.y + EditorGUIUtility.singleLineHeight + gap, rowHeader.width - 12f, valueHeight);
                        DrawFlattened(valueRect, valueProp);

                        y += combinedH;
                    }
                    else
                    {
                        // Collapsed: draw one header box and label; toggle on MouseUp (no focus steal)
                        GUI.Box(rowHeader, GUIContent.none, EditorStyles.helpBox);
                        var headerLabel = new Rect(rowHeader.x + 6f, rowHeader.y + 2f, rowHeader.width - 12f, rowHeader.height);
                        EditorGUI.LabelField(headerLabel, enumCache.labels[i]);

                        if (e.type == EventType.MouseUp && e.button == 0 && rowHeader.Contains(e.mousePosition))
                        {
                            SetExpanded(rowKey, true);
                            e.Use();
                        }

                        y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    }
                }
                else
                {
                    // Single-line: key label left, value field right
                    float keyWidth = position.width * SPLIT_KEY_WIDTH_RATIO;
                    float valueWidth = position.width - keyWidth;

                    var keyRect = new Rect(position.x + indentOffset, y, keyWidth - indentOffset, EditorGUIUtility.singleLineHeight);
                    var valRect = new Rect(position.x + indentOffset + keyWidth, y, valueWidth, EditorGUIUtility.singleLineHeight);

                    EditorGUI.LabelField(keyRect, enumCache.labels[i]);

                    // Stable control name for focus
                    GUI.SetNextControlName(valueProp.propertyPath);
                    EditorGUI.PropertyField(valRect, valueProp, SNone, true);

                    y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            // Optional: allow Escape to clear focus
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
            {
                GUIUtility.keyboardControl = 0;
                GUIUtility.hotControl = 0;
                Event.current.Use();
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
                    vals[i] = (int)Convert.ChangeType(valuesArr.GetValue(i), typeof(int));
                    labels[i] = ObjectNames.NicifyVariableName(valuesArr.GetValue(i).ToString());
                }
                cache = new EnumCache { values = vals, labels = labels };
                SEnumCache[enumType] = cache;
            }
            return cache;
        }

        // Expansion state (session cache + EditorPrefs on change)
        static bool GetExpanded(string key, bool defaultValue)
        {
            if (SExpanded.TryGetValue(key, out bool v)) return v;
            v = EditorPrefs.GetBool(key, defaultValue);
            SExpanded[key] = v;
            return v;
        }
        static void SetExpanded(string key, bool value)
        {
            if (!SExpanded.TryGetValue(key, out var cur) || cur != value)
            {
                SExpanded[key] = value;
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

        // Draw complex properties flattened, with stable control names
        static void DrawFlattened(Rect rect, SerializedProperty prop)
        {
            if (prop == null) return;

            if (prop.propertyType != SerializedPropertyType.Generic)
            {
                GUI.SetNextControlName(prop.propertyPath);
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
                var r = new Rect(x, y, w, h);
                GUI.SetNextControlName(iter.propertyPath); // stable focus per field
                EditorGUI.PropertyField(r, iter, false);
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
            if (keysProperty.arraySize != enumInts.Length)
                keysProperty.arraySize = enumInts.Length;

            for (int i = 0; i < enumInts.Length; i++)
            {
                var elem = keysProperty.GetArrayElementAtIndex(i);
                if (elem.intValue != enumInts[i]) elem.intValue = enumInts[i];
            }

            // Keep values sized; no Reset
            if (valuesProperty.arraySize != enumInts.Length)
                valuesProperty.arraySize = enumInts.Length;
        }

        static string GetDictKey(SerializedProperty property)
        {
            return property.serializedObject.targetObject.GetInstanceID() + "_" + property.propertyPath;
        }
    }
#endif
}
