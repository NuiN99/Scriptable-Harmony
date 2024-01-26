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
        FindScriptableObjectGUI _findGUI;

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

            window._currentTab = tab;
            window.Show();
        }

        public static void OpenFindWindow(string typeName, SerializedProperty property)
        {
            Open(Tab.FindSO);
            instance._findGUI = new FindScriptableObjectGUI(typeName, property, true);
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
            _findGUI ??= new FindScriptableObjectGUI("", null, false);
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
                    CreateScriptableObjectGUI();
                    break;
                case Tab.FindSO:
                    FindScriptableObjectGUI();
                    break;
                case Tab.CreateType:
                    GenerateCustomTypeWindowGUI();
                    break;
                case Tab.Logger:
                    SHLoggerOptionsWindowGUI();
                    break;
            }
            
            if (_currentTab != Tab.FindSO) _findGUI.openedFromField = false;
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

        void CreateScriptableObjectGUI()
        {
            _createGUI.DrawGUI();
        }

        void FindScriptableObjectGUI()
        {
            _findGUI.DrawGUI(this);
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