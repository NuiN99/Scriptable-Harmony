#if UNITY_EDITOR

using NuiN.ScriptableHarmony.Core;
using NuiN.ScriptableHarmony.Sound;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Editor
{
    internal class ScriptableHarmonyWindow : EditorWindow
    {
        public enum Tab
        {
            CreateSO,
            FindSO,
            CreateType,
            Logger,
            Volume
        }

        static ScriptableHarmonyWindow instance;

        const string PREFS_LAST_TAB = "ScriptableHarmonyWindow_LastTab";

        SelectionPathController _pathController;
        Tab _currentTab = Tab.CreateSO;

        CreateScriptableObjectGUI _createGUI;
        FindScriptableObjectGUI _findGUI;
        GenerateCustomTypeGUI _generateTypeGUI;
        SHLoggerSettingsGUI _loggerSettingsGUI;

        [MenuItem("ScriptableHarmony/Open Wizard")]
        static void Open()
        {
            ScriptableHarmonyWindow window = GetWindow<ScriptableHarmonyWindow>("ScriptableHarmony Editor");
            instance = window;

            window._currentTab = EditorPrefs.HasKey(PREFS_LAST_TAB) ? (Tab)EditorPrefs.GetInt(PREFS_LAST_TAB) : default;
            window.Show();
        }

        public static void Open(Tab tab)
        {
            ScriptableHarmonyWindow window = GetWindow<ScriptableHarmonyWindow>("ScriptableHarmony Editor");
            
            instance = window;

            window._currentTab = tab;
            window.Show();
        }

        public static void OpenFindWindow(string typeName, SerializedProperty property, SOType soType)
        {
            Open(Tab.FindSO);
            instance._findGUI = new FindScriptableObjectGUI(typeName, property, true, soType);
        }

        void OnLostFocus()
        {
            if(_findGUI.openedFromField) Close();
        }

        void OnEnable()
        {
            instance = this;
            
            _pathController = new SelectionPathController(this);
            
            _createGUI = new CreateScriptableObjectGUI(_pathController);
            _generateTypeGUI = new GenerateCustomTypeGUI(_pathController);
            _loggerSettingsGUI = new SHLoggerSettingsGUI();
            
            _findGUI ??= new FindScriptableObjectGUI("", null, false, SOType.Variable);
        }
        void OnDisable()
        {
            _pathController?.Dispose();
        }

        void OnGUI()
        {
            DisplayTabs();
            
            switch (_currentTab)
            {
                case Tab.CreateSO: _createGUI.DrawGUI(); break;
                case Tab.FindSO: _findGUI.DrawGUI(this); break;
                case Tab.CreateType: _generateTypeGUI.DrawGUI(this); break;
                case Tab.Logger: _loggerSettingsGUI.DrawGUI(); break;
                case Tab.Volume: MasterVolumeManager.DrawSliderGUI(); break;
            }
            
            if (_currentTab != Tab.FindSO) _findGUI.openedFromField = false;
            
            return;

            void DisplayTabs()
            {
                GUILayout.BeginHorizontal(EditorStyles.toolbar);
            
                if (GUILayout.Toggle(_currentTab == Tab.CreateSO, "Create", EditorStyles.toolbarButton))
                    _currentTab = Tab.CreateSO;

                if (GUILayout.Toggle(_currentTab == Tab.FindSO, "Find", EditorStyles.toolbarButton))
                    _currentTab = Tab.FindSO;

                if (GUILayout.Toggle(_currentTab == Tab.CreateType, "Generate Type", EditorStyles.toolbarButton))
                    _currentTab = Tab.CreateType;

                if (GUILayout.Toggle(_currentTab == Tab.Logger, "Logger", EditorStyles.toolbarButton))
                    _currentTab = Tab.Logger;

                if (GUILayout.Toggle(_currentTab == Tab.Volume, "Volume", EditorStyles.toolbarButton))
                    _currentTab = Tab.Volume;

                GUILayout.EndHorizontal();
                
                EditorPrefs.SetInt(PREFS_LAST_TAB, (int)_currentTab);
            }
        }
    }
}

#endif