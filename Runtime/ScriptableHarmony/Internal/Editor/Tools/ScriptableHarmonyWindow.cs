using NuiN.ScriptableHarmony.Core;
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
            Logger
        }

        static ScriptableHarmonyWindow instance;

        SelectionPathController _pathController;
        static Tab currentTab = Tab.CreateSO;

        CreateScriptableObjectGUI _createGUI;
        FindScriptableObjectGUI _findGUI;
        GenerateCustomTypeGUI _generateTypeGUI;
        SHLoggerSettingsGUI _loggerSettingsGUI;

        [MenuItem("ScriptableHarmony/Open Editor")]
        static void Open()
        {
            ScriptableHarmonyWindow window = GetWindow<ScriptableHarmonyWindow>("ScriptableHarmony Editor");
            instance = window;
            window.Show();
        }

        public static void Open(Tab tab)
        {
            ScriptableHarmonyWindow window = GetWindow<ScriptableHarmonyWindow>("ScriptableHarmony Editor");
            
            instance = window;

            currentTab = tab;
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
            
            switch (currentTab)
            {
                case Tab.CreateSO: _createGUI.DrawGUI(); break;
                case Tab.FindSO: _findGUI.DrawGUI(this); break;
                case Tab.CreateType: _generateTypeGUI.DrawGUI(this); break;
                case Tab.Logger: _loggerSettingsGUI.DrawGUI(); break;
            }
            
            if (currentTab != Tab.FindSO) _findGUI.openedFromField = false;
            
            return;

            void DisplayTabs()
            {
                GUILayout.BeginHorizontal(EditorStyles.toolbar);
            
                if (GUILayout.Toggle(currentTab == Tab.CreateSO, "Create", EditorStyles.toolbarButton))
                    currentTab = Tab.CreateSO;

                if (GUILayout.Toggle(currentTab == Tab.FindSO, "Find", EditorStyles.toolbarButton))
                    currentTab = Tab.FindSO;

                if (GUILayout.Toggle(currentTab == Tab.CreateType, "Generate Type", EditorStyles.toolbarButton))
                    currentTab = Tab.CreateType;

                if (GUILayout.Toggle(currentTab == Tab.Logger, "Logger", EditorStyles.toolbarButton))
                    currentTab = Tab.Logger;

                GUILayout.EndHorizontal();
            }
        }
    }
}