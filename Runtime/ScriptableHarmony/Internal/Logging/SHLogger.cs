using System.Diagnostics;
using NuiN.ScriptableHarmony.Base;
using NuiN.ScriptableHarmony.Editor;
using NuiN.ScriptableHarmony.Internal.Helpers;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace NuiN.ScriptableHarmony.Internal.Logging
{
    internal static class SHLogger
    {
        public static bool logging = true;
        
        public static bool variableLogging = true;
        public static bool listVariableLogging = true;
        public static bool runtimeSetLogging = true;
        public static bool runtimeSingleLogging = true;

        const string VARIABLE_COLOR = "#b980ff";
        const string LIST_VARIABLE_COLOR = "#8c98ff";
        const string RUNTIME_SET_COLOR = "#bbf0c3";
        const string RUNTIME_SINGLE_COLOR = "#4e9159";

        const string POSITIVE_COLOR = "#9591ff";
        const string NEGATIVE_COLOR = "#ff6e6e";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void SetLoggingState()
        {
            logging = SHLoggerSettings.IsLoggingEnabled();
            variableLogging = SHLoggerSettings.IsVariableLoggingEnabled();
            listVariableLogging = SHLoggerSettings.IsListVariableLoggingEnabled();
            runtimeSetLogging = SHLoggerSettings.IsRuntimeSetLoggingEnabled();
            runtimeSingleLogging = SHLoggerSettings.IsRuntimeSingleLoggingEnabled();
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("UNITY_EDITOR")] [Conditional("DEVELOPMENT_BUILD")]
        static void Log<T>(string message, SOType type, EditorObjectSO<T> obj)
        {
            if (!logging || !obj.LogActions) return;
            switch (type)
            {
                case SOType.Variable when !variableLogging:
                case SOType.List when !listVariableLogging:
                case SOType.RuntimeSet when !runtimeSetLogging:
                case SOType.RuntimeSingle when !runtimeSingleLogging:
                    return;
                default:
                {
                    string soType = type.ToString();
                    Debug.Log($"<b><color='{GetColor(type)}'>{soType}</color></b> : <b><color='white'>{obj.name}</color></b> | {message}", obj);
                    break;
                }
            }
        }

        [Conditional("UNITY_EDITOR")] [Conditional("DEVELOPMENT_BUILD")]
        static void LogAction<T>(string eventName, SOType type, string contents, bool invokedAction, EditorObjectSO<T> obj)
        {
            string suffix = invokedAction ? "" : " (No Invoke)";
            Log($"<b><color='orange'>{eventName}{suffix}</color></b> | {contents}", type, obj);
        }

        [Conditional("UNITY_EDITOR")] [Conditional("DEVELOPMENT_BUILD")]
        public static void LogSet<T>(string eventName, SOType type,  string fromString, string toString, bool invokedAction, EditorObjectSO<T> obj)
        {
            fromString = string.IsNullOrEmpty(fromString) ? "<color='#858585'>null</color>" : fromString;
            toString = string.IsNullOrEmpty(toString) ? "<color='#858585'>null</color>" : toString;

            string from = $"From: <b><color='{NEGATIVE_COLOR}'>{fromString}</color></b>";
            string to = $"To: <b><color='{POSITIVE_COLOR}'>{toString}</color></b>";
            string contents = $"{from} | {to}";
            LogAction(eventName, type, contents, invokedAction, obj);
        }
        
        [Conditional("UNITY_EDITOR")] [Conditional("DEVELOPMENT_BUILD")]
        public static void LogAddRemove<T>(string eventName, SOType type, string itemName, bool added, bool invokedAction, EditorObjectSO<T> obj)
        {
            string color = added ? POSITIVE_COLOR : NEGATIVE_COLOR;
            string contents = $"<b><color='{color}'>{itemName}</color></b>";
            LogAction(eventName, type, contents, invokedAction, obj);
        }
        
        [Conditional("UNITY_EDITOR")] [Conditional("DEVELOPMENT_BUILD")]
        public static void LogReplacedCleared<T>(string eventName, SOType type, int oldCount, int newCount, bool invokedAction, EditorObjectSO<T> obj)
        {
            string contents = $"Old Count:<b><color='red'>{oldCount}</color></b> | New Count:<b><color='white'>{newCount}</color></b>";
            LogAction(eventName, type, contents, invokedAction, obj);
        }

        static string GetColor(SOType type)
        {
            return type switch
            {
                SOType.Variable => VARIABLE_COLOR,
                SOType.List => LIST_VARIABLE_COLOR,
                SOType.RuntimeSet => RUNTIME_SET_COLOR,
                SOType.RuntimeSingle => RUNTIME_SINGLE_COLOR,
                _ => "white"
            };
        }
    }
}

