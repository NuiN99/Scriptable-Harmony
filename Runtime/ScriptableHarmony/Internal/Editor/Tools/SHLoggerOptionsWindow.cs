using NuiN.ScriptableHarmony.Internal.Logging;
using UnityEditor;
using UnityEngine;

namespace  NuiN.ScriptableHarmony.Editor
{
#if UNITY_EDITOR
    internal class SHLoggerOptionsWindow : EditorWindow
    {
        bool _loggingEnabled = true;
        bool _variableLoggingEnabled = true;
        bool _listVariableLoggingEnabled = true;
        bool _runtimeSetLoggingEnabled = true;
        bool _runtimeSingleLoggingEnabled = true;

        [MenuItem("ScriptableHarmony/Logger Options")]
        static void OpenWindow()
        {
            SHLoggerOptionsWindow window = (SHLoggerOptionsWindow)EditorWindow.GetWindow(typeof(SHLoggerOptionsWindow));
            window.titleContent = new GUIContent("Logger Options");
            window.minSize = new Vector2(180, 175);
            window.Show();
        }

        void OnEnable()
        {
            _loggingEnabled = SHLoggerSettings.IsLoggingEnabled();
            _variableLoggingEnabled = SHLoggerSettings.IsVariableLoggingEnabled();
            _listVariableLoggingEnabled = SHLoggerSettings.IsListVariableLoggingEnabled();
            _runtimeSetLoggingEnabled = SHLoggerSettings.IsRuntimeSetLoggingEnabled();
            _runtimeSingleLoggingEnabled = SHLoggerSettings.IsRuntimeSingleLoggingEnabled();
        }

        void OnGUI()
        {
            GUILayout.Label("Global", EditorStyles.boldLabel);
            _loggingEnabled = EditorGUILayout.Toggle("Logging Enabled", _loggingEnabled);
            
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            GUILayout.Label("Individual", EditorStyles.boldLabel);
            _variableLoggingEnabled = EditorGUILayout.Toggle("Variable Logs", _variableLoggingEnabled);
            _listVariableLoggingEnabled = EditorGUILayout.Toggle("List Variable Logs", _listVariableLoggingEnabled);
            _runtimeSetLoggingEnabled = EditorGUILayout.Toggle("Runtime Set Logs", _runtimeSetLoggingEnabled);
            _runtimeSingleLoggingEnabled = EditorGUILayout.Toggle("Runtime Single Logs", _runtimeSingleLoggingEnabled);

            if (!GUI.changed) return;
            
            SetLoggingOption(SHLoggerSettings.PREFS_LOGGING_KEY, _loggingEnabled);
            SHLogger.logging = _loggingEnabled;
            SetLoggingOption(SHLoggerSettings.PREFS_VARIABLE_LOGGING_KEY, _variableLoggingEnabled);
            SHLogger.variableLogging = _variableLoggingEnabled;
            SetLoggingOption(SHLoggerSettings.PREFS_LISTVARIABLE_LOGGING_KEY, _listVariableLoggingEnabled);
            SHLogger.listVariableLogging = _listVariableLoggingEnabled;
            SetLoggingOption(SHLoggerSettings.PREFS_RUNTIMESET_LOGGING_KEY, _runtimeSetLoggingEnabled);
            SHLogger.runtimeSetLogging = _runtimeSetLoggingEnabled;
            SetLoggingOption(SHLoggerSettings.PREFS_RUNTIMESINGLE_LOGGING_KEY, _runtimeSingleLoggingEnabled);
            SHLogger.runtimeSingleLogging = _runtimeSingleLoggingEnabled;
        }

        static void SetLoggingOption(string prefsKey, bool enabled)
        {
            PlayerPrefs.SetInt(prefsKey, enabled ? 1 : 0);
            PlayerPrefs.Save();
        }


        
    }
#endif

    public static class SHLoggerSettings
    {
        public const string PREFS_LOGGING_KEY = "SHLoggerBool";

        public const string PREFS_VARIABLE_LOGGING_KEY = "SHLoggerVariableBool";
        public const string PREFS_LISTVARIABLE_LOGGING_KEY = "SHLoggerListVariableBool";
        public const string PREFS_RUNTIMESET_LOGGING_KEY = "SHLoggerRuntimeSetBool";
        public const string PREFS_RUNTIMESINGLE_LOGGING_KEY = "SHLoggeRuntimeSingleBool";
        
        static bool LoggingCheck(string prefsKey)
        {
            if (PlayerPrefs.HasKey(prefsKey)) return PlayerPrefs.GetInt(prefsKey) == 1;

            PlayerPrefs.SetInt(prefsKey, 1);
            PlayerPrefs.Save();
            return true;
        }
        
        public static bool IsLoggingEnabled() => LoggingCheck(PREFS_LOGGING_KEY);
        public static bool IsVariableLoggingEnabled() => LoggingCheck(PREFS_VARIABLE_LOGGING_KEY);
        public static bool IsListVariableLoggingEnabled() => LoggingCheck(PREFS_LISTVARIABLE_LOGGING_KEY);
        public static bool IsRuntimeSetLoggingEnabled() => LoggingCheck(PREFS_RUNTIMESET_LOGGING_KEY);
        public static bool IsRuntimeSingleLoggingEnabled() => LoggingCheck(PREFS_RUNTIMESINGLE_LOGGING_KEY);
    }
}



