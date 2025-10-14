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
        const float INDENT_PER_LEVEL = 20f;
        const float SPLIT_KEY_WIDTH_RATIO = 0.25f;

        // Session caches
        static readonly Dictionary<Type, EnumCache> SEnumCache = new();
        static readonly Dictionary<string, bool> SExpanded = new(); // header + rows
        static readonly Dictionary<string, float[]> SRowHeights = new(); // per dictionary -> per-index value heights
        static readonly GUIContent SNone = GUIContent.none;

        // Header visuals
        static readonly GUIStyle HeaderBox = new GUIStyle(EditorStyles.helpBox)
        {
            padding = new RectOffset(4, 4, 3, 3),
            alignment = TextAnchor.MiddleLeft
        };
        static readonly GUIStyle HeaderText = new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleLeft,
            clipping = TextClipping.Clip,
            wordWrap = false
        };

        struct EnumCache
        {
            public int[] values;
            public string[] labels;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            string dictKey = GetDictKey(property);
            string controlPrefix = dictKey + "|";
            bool dictExpanded = GetExpanded(dictKey, false);

            // Dictionary header
            var dictHeader = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            GUI.Box(dictHeader, GUIContent.none, HeaderBox);

            float lh = EditorGUIUtility.singleLineHeight;
            float dictTextY = dictHeader.y + Mathf.Max(0f, (dictHeader.height - lh) * 0.5f);
            var dictLabelRect = new Rect(dictHeader.x + 6f, dictTextY, dictHeader.width - 12f, lh);
            EditorGUI.LabelField(dictLabelRect, label, HeaderText);

            var e = Event.current;
            if (IsAltClick(e, dictHeader))
            {
                // Clear focus to prevent caret jump
                GUIUtility.keyboardControl = 0;
                GUIUtility.hotControl = 0;

                // Decide mass action from current state
                bool expandAll = !dictExpanded;

                var valuesProp = property.FindPropertyRelative("values");
                int rowCount = valuesProp?.arraySize ?? 0;

                if (expandAll)
                {
                    // Open container and expand all rows
                    dictExpanded = true;
                    SetExpanded(dictKey, true);
                    SetAllRowsExpanded(dictKey, rowCount, true);
                }
                else
                {
                    // Collapse all rows AND close the container
                    SetAllRowsExpanded(dictKey, rowCount, false);
                    dictExpanded = false;
                    SetExpanded(dictKey, false);
                }

                e.Use();
            }
            else if (IsPlainClick(e, dictHeader))
            {
                GUIUtility.keyboardControl = 0;
                GUIUtility.hotControl = 0;

                dictExpanded = !dictExpanded;
                SetExpanded(dictKey, dictExpanded);
                e.Use();
            }

            if (!dictExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            // Resolve enum cache
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

            if (keysProperty.arraySize != enumCache.values.Length)
                SyncEnumWithKeys(keysProperty, valuesProperty, enumCache.values);

            float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            int indentLevel = EditorGUI.indentLevel + 1;
            float indentOffset = indentLevel * INDENT_PER_LEVEL;

            float[] rowHeights = GetOrResizeRowHeights(dictKey, valuesProperty.arraySize);

            for (int i = 0; i < keysProperty.arraySize; i++)
            {
                var valueProp = valuesProperty.GetArrayElementAtIndex(i);

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
                    bool rowExpanded = GetExpanded(rowKey, false);

                    var rowHeader = new Rect(position.x + indentOffset, y, position.width - indentOffset, EditorGUIUtility.singleLineHeight);

                    if (rowExpanded)
                    {
                        float gap = EditorGUIUtility.standardVerticalSpacing;
                        float combinedH = EditorGUIUtility.singleLineHeight + gap + valueHeight + EditorGUIUtility.standardVerticalSpacing;

                        var combinedRect = new Rect(rowHeader.x, rowHeader.y, rowHeader.width, combinedH);
                        GUI.Box(combinedRect, GUIContent.none, HeaderBox);

                        float textY = rowHeader.y + Mathf.Max(0f, (rowHeader.height - lh) * 0.5f);
                        var headerLabel = new Rect(rowHeader.x + 6f, textY, rowHeader.width - 12f, lh);
                        EditorGUI.LabelField(headerLabel, enumCache.labels[i], HeaderText);

                        if (IsAltClick(e, rowHeader))
                        {
                            GUIUtility.keyboardControl = 0;
                            GUIUtility.hotControl = 0;

                            // Collapse all rows
                            int rowCount = valuesProperty.arraySize;
                            SetAllRowsExpanded(dictKey, rowCount, false);
                            e.Use();
                        }
                        else if (IsPlainClick(e, rowHeader))
                        {
                            GUIUtility.keyboardControl = 0;
                            GUIUtility.hotControl = 0;

                            SetExpanded(rowKey, false);
                            e.Use();
                        }

                        var valueRect = new Rect(rowHeader.x + 6f, rowHeader.y + EditorGUIUtility.singleLineHeight + gap, rowHeader.width - 12f, valueHeight);
                        DrawFlattened(valueRect, valueProp, controlPrefix);

                        y += combinedH;
                    }
                    else
                    {
                        GUI.Box(rowHeader, GUIContent.none, HeaderBox);
                        float textY = rowHeader.y + Mathf.Max(0f, (rowHeader.height - lh) * 0.5f);
                        var headerLabel = new Rect(rowHeader.x + 6f, textY, rowHeader.width - 12f, lh);
                        EditorGUI.LabelField(headerLabel, enumCache.labels[i], HeaderText);

                        if (IsAltClick(e, rowHeader))
                        {
                            GUIUtility.keyboardControl = 0;
                            GUIUtility.hotControl = 0;

                            // Expand all rows
                            int rowCount = valuesProperty.arraySize;
                            SetAllRowsExpanded(dictKey, rowCount, true);
                            // Ensure dictionary stays open
                            SetExpanded(dictKey, true);
                            e.Use();
                        }
                        else if (IsPlainClick(e, rowHeader))
                        {
                            GUIUtility.keyboardControl = 0;
                            GUIUtility.hotControl = 0;

                            SetExpanded(rowKey, true);
                            e.Use();
                        }

                        y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    }
                }
                else
                {
                    // Single-line row
                    float keyWidth = position.width * SPLIT_KEY_WIDTH_RATIO;
                    float valueWidth = position.width - keyWidth;

                    var keyRect = new Rect(position.x + indentOffset, y, keyWidth - indentOffset, EditorGUIUtility.singleLineHeight);
                    var valRect = new Rect(position.x + indentOffset + keyWidth, y, valueWidth, EditorGUIUtility.singleLineHeight);

                    float textY = keyRect.y + Mathf.Max(0f, (keyRect.height - lh) * 0.5f);
                    var keyTextRect = new Rect(keyRect.x, textY, keyRect.width, lh);
                    EditorGUI.LabelField(keyTextRect, enumCache.labels[i], HeaderText);

                    GUI.SetNextControlName(controlPrefix + valueProp.propertyPath);
                    EditorGUI.PropertyField(valRect, valueProp, SNone, true);

                    y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            // Optional: Escape clears focus
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
            bool dictExpanded = GetExpanded(dictKey, false);

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
                    if (GetExpanded(rowKey, false))
                        total += vHeight + EditorGUIUtility.standardVerticalSpacing;
                }
                else
                {
                    total += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return total;
        }

        // Helpers for clicks
        static bool IsAltClick(Event e, Rect r)
        {
            return e.type == EventType.MouseUp && e.button == 0 && r.Contains(e.mousePosition) &&
                   (e.alt || e.modifiers == EventModifiers.Alt);
        }
        static bool IsPlainClick(Event e, Rect r)
        {
            return e.type == EventType.MouseUp && e.button == 0 && r.Contains(e.mousePosition) &&
                   !e.alt && e.modifiers == EventModifiers.None;
        }
        static void SetAllRowsExpanded(string dictKey, int rowCount, bool expanded)
        {
            for (int i = 0; i < rowCount; i++)
            {
                string rowKey = dictKey + "_row_" + i;
                SetExpanded(rowKey, expanded);
            }
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
        static void DrawFlattened(Rect rect, SerializedProperty prop, string controlPrefix)
        {
            if (prop == null) return;

            if (prop.propertyType != SerializedPropertyType.Generic)
            {
                GUI.SetNextControlName(controlPrefix + prop.propertyPath);
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

                GUI.SetNextControlName(controlPrefix + iter.propertyPath);
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
