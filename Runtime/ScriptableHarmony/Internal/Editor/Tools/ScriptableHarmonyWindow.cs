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
        Tab _currentTab = Tab.CreateSO;

        CreateScriptableObjectGUI _createGUI;

        [MenuItem("ScriptableHarmony/Open Editor")]
        static void Open()
        {
            ScriptableHarmonyWindow window = GetWindow<ScriptableHarmonyWindow>("ScriptableHarmony Editor");
            window.Show();
        }

        public static void Open(Tab tab)
        {
            instance._currentTab = tab;
            ScriptableHarmonyWindow window = GetWindow<ScriptableHarmonyWindow>("ScriptableHarmony Editor");
            window.Show();
        }

        void OnEnable()
        {
            instance = this;
            
            _pathController = new SelectionPathController(this);
            
            _createGUI = new CreateScriptableObjectGUI(_pathController);
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
                case Tab.CreateSO:
                    CreateSOWindowGUI();
                    break;
                case Tab.FindSO:
                    FindScriptableObjectWindowGUI();
                    break;
                case Tab.CreateType:
                    GenerateCustomTypeWindowGUI();
                    break;
                case Tab.Logger:
                    SHLoggerOptionsWindowGUI();
                    break;
            }
        }

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
        }

        void CreateSOWindowGUI()
        {
            
            _createGUI.DrawGUI();
        }

        void FindScriptableObjectWindowGUI()
        {
            // Include the contents of the FindScriptableObjectWindow class here
            // ...
        }

        void GenerateCustomTypeWindowGUI()
        {
            // Include the contents of the GenerateCustomTypeWindow class here
            // ...
        }

        void SHLoggerOptionsWindowGUI()
        {
            // Include the contents of the SHLoggerOptionsWindow class here
            // ...
        }
    }
}