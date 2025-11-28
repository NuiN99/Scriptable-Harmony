#if UNITY_EDITOR

using NuiN.NExtensions.Editor;
using NuiN.ScriptableHarmony.Core;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Editor
{
    internal class ScriptableHarmonyWizard : EditorWindow
    {
        public enum Tab
        {
            CreateSO,
            FindSO,
            CreateType,
            Logger,
        }

        static ScriptableHarmonyWizard instance;

        const string PREFS_LAST_TAB = "ScriptableHarmonyWindow_LastTab";

        Tab _currentTab = Tab.CreateSO;

        CreateScriptableObjectGUI _createGUI;
        FindScriptableObjectGUI _findGUI;
        GenerateCustomTypeGUI _generateTypeGUI;
        SHLoggerSettingsGUI _loggerSettingsGUI;

        [MenuItem("Tools/SH/Open Wizard")]
        static void Open()
        {
            ScriptableHarmonyWizard window = GetWindow<ScriptableHarmonyWizard>("ScriptableHarmony Editor");
            instance = window;

            window._currentTab = EditorPrefs.HasKey(PREFS_LAST_TAB) ? (Tab)EditorPrefs.GetInt(PREFS_LAST_TAB) : default;
            window.Show();
        }

        public static void Open(Tab tab)
        {
            ScriptableHarmonyWizard window = GetWindow<ScriptableHarmonyWizard>("ScriptableHarmony Editor");
            
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
            
            _createGUI = new CreateScriptableObjectGUI();
            _generateTypeGUI = new GenerateCustomTypeGUI();
            _loggerSettingsGUI = new SHLoggerSettingsGUI();
            
            
            _findGUI ??= new FindScriptableObjectGUI("", null, false, SOType.Variable);
        }

        void OnGUI()
        {
            DisplayTabs();
            
            switch (_currentTab)
            {
                case Tab.CreateSO: _createGUI.DrawGUI(this); break;
                case Tab.FindSO: _findGUI.DrawGUI(this); break;
                case Tab.CreateType: _generateTypeGUI.DrawGUI(this); break;
                case Tab.Logger: _loggerSettingsGUI.DrawGUI(); break;
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

                GUILayout.EndHorizontal();
                
                EditorPrefs.SetInt(PREFS_LAST_TAB, (int)_currentTab);
            }
        }

        public static void DrawSelectionPathGUI(EditorWindow window)
        {
            EditorGUI.BeginDisabledGroup(true);
            GUILayout.BeginHorizontal();
            
            GUILayout.Label("Path:", GUILayout.Width(50));
            string path = SelectionPath.GetPath();
            EditorGUILayout.TextField(path, GUILayout.ExpandWidth(true));
            
            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
            
            window.Repaint();
        }
    }
}

#endif