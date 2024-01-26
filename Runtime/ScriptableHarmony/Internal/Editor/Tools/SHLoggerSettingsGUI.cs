using NuiN.ScriptableHarmony.Core;
using UnityEditor;
using UnityEngine;

namespace  NuiN.ScriptableHarmony.Editor
{
#if UNITY_EDITOR
    internal class SHLoggerSettingsGUI
    {
        bool _loggingEnabled = SHLoggerSettings.IsLoggingEnabled();
        bool _variableLoggingEnabled = SHLoggerSettings.IsVariableLoggingEnabled();
        bool _listVariableLoggingEnabled = SHLoggerSettings.IsListVariableLoggingEnabled();
        bool _dictionaryVariableLoggingEnabled = SHLoggerSettings.IsListVariableLoggingEnabled();
        bool _runtimeSetLoggingEnabled = SHLoggerSettings.IsRuntimeSetLoggingEnabled();
        bool _runtimeSingleLoggingEnabled = SHLoggerSettings.IsRuntimeSingleLoggingEnabled();

        public void DrawGUI()
        {
            GUILayout.Label("Global Logging", EditorStyles.boldLabel);
            DrawHorizontalLine(2);
            _loggingEnabled = GUILayout.Toggle(_loggingEnabled, "Logging Enabled", GUILayout.ExpandWidth(true));
            DrawHorizontalLine();
            
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            GUILayout.Label("Individual Logging", EditorStyles.boldLabel);

            DrawHorizontalLine(2);
            _variableLoggingEnabled = GUILayout.Toggle(_variableLoggingEnabled, "Scriptable Variable", GUILayout.ExpandWidth(true));
            DrawHorizontalLine();
            _listVariableLoggingEnabled = GUILayout.Toggle(_listVariableLoggingEnabled, "Scriptable List", GUILayout.ExpandWidth(true));
            DrawHorizontalLine();
            _dictionaryVariableLoggingEnabled = GUILayout.Toggle(_dictionaryVariableLoggingEnabled, "Scriptable Dictionary", GUILayout.ExpandWidth(true));
            DrawHorizontalLine();
            _runtimeSetLoggingEnabled = GUILayout.Toggle(_runtimeSetLoggingEnabled, "Runtime Set", GUILayout.ExpandWidth(true));
            DrawHorizontalLine();
            _runtimeSingleLoggingEnabled = GUILayout.Toggle(_runtimeSingleLoggingEnabled, "Runtime Single", GUILayout.ExpandWidth(true));
            DrawHorizontalLine();

            if (!GUI.changed) return;
            
            SetLoggingOption(SHLoggerSettings.PREFS_LOGGING_KEY, _loggingEnabled);
            SHLogger.logging = _loggingEnabled;
            
            SetLoggingOption(SHLoggerSettings.PREFS_VARIABLE_LOGGING_KEY, _variableLoggingEnabled);
            SHLogger.variableLogging = _variableLoggingEnabled;
            
            SetLoggingOption(SHLoggerSettings.PREFS_LISTVARIABLE_LOGGING_KEY, _listVariableLoggingEnabled);
            SHLogger.listVariableLogging = _listVariableLoggingEnabled;
            
            SetLoggingOption(SHLoggerSettings.PREFS_DICTIONARYVARIABLE_LOGGING_KEY, _dictionaryVariableLoggingEnabled);
            SHLogger.dictionaryVariableLogging = _listVariableLoggingEnabled;
            
            SetLoggingOption(SHLoggerSettings.PREFS_RUNTIMESET_LOGGING_KEY, _runtimeSetLoggingEnabled);
            SHLogger.runtimeSetLogging = _runtimeSetLoggingEnabled;
            
            SetLoggingOption(SHLoggerSettings.PREFS_RUNTIMESINGLE_LOGGING_KEY, _runtimeSingleLoggingEnabled);
            SHLogger.runtimeSingleLogging = _runtimeSingleLoggingEnabled;

            return;
            
            void DrawHorizontalLine(float height = 1)
            {
                Rect rect = EditorGUILayout.GetControlRect(false, height);
                EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
            }
        }

        static void SetLoggingOption(string prefsKey, bool enabled)
        {
            EditorPrefs.SetInt(prefsKey, enabled ? 1 : 0);
        }
    }
#endif
    
    public static class SHLoggerSettings
    {
        public const string PREFS_LOGGING_KEY = "SHLoggerBool";

        public const string PREFS_VARIABLE_LOGGING_KEY = "SHLoggerVariableBool";
        public const string PREFS_LISTVARIABLE_LOGGING_KEY = "SHLoggerListVariableBool";
        public const string PREFS_DICTIONARYVARIABLE_LOGGING_KEY = "SHLoggerDictionaryVariableBool";
        public const string PREFS_RUNTIMESET_LOGGING_KEY = "SHLoggerRuntimeSetBool";
        public const string PREFS_RUNTIMESINGLE_LOGGING_KEY = "SHLoggeRuntimeSingleBool";
        
        static bool LoggingCheck(string prefsKey)
        {
#if UNITY_EDITOR
            if (EditorPrefs.HasKey(prefsKey)) return EditorPrefs.GetInt(prefsKey) == 1;

            EditorPrefs.SetInt(prefsKey, 1);
#endif
            return true;
        }
        
        public static bool IsLoggingEnabled() => LoggingCheck(PREFS_LOGGING_KEY);
        public static bool IsVariableLoggingEnabled() => LoggingCheck(PREFS_VARIABLE_LOGGING_KEY);
        public static bool IsListVariableLoggingEnabled() => LoggingCheck(PREFS_LISTVARIABLE_LOGGING_KEY);
        public static bool IsDictionaryVariableLoggingEnabled() => LoggingCheck(PREFS_LISTVARIABLE_LOGGING_KEY);
        public static bool IsRuntimeSetLoggingEnabled() => LoggingCheck(PREFS_RUNTIMESET_LOGGING_KEY);
        public static bool IsRuntimeSingleLoggingEnabled() => LoggingCheck(PREFS_RUNTIMESINGLE_LOGGING_KEY);
    }
}



