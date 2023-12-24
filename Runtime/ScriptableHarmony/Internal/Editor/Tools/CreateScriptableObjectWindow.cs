using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using NuiN.ScriptableHarmony.Internal.Helpers;
using NuiN.ScriptableHarmony.ListVariable.Base;
using NuiN.ScriptableHarmony.RuntimeSet.Base;
using NuiN.ScriptableHarmony.RuntimeSingle.Base;
using NuiN.ScriptableHarmony.Variable.Base;

namespace NuiN.ScriptableHarmony.Core.Editor.Tools
{
#if UNITY_EDITOR
    internal class CreateScriptableObjectWindow : EditorWindow
    {
        Type[] _scriptTypes;
        SOType _selectedSOType = SOType.Variable;
        string _scriptTypeSearch = "";
        string _assetName;
        Vector2 _scrollPosition;

        SelectionPathController _pathController;

        [MenuItem("ScriptableHarmony/Create a Scriptable Object")]
        public static void ShowWindow()
        {
            GetWindow<CreateScriptableObjectWindow>("Scriptable Object Creator");
        }

        void OnEnable()
        {
            InitializeScriptTypes();
            _pathController = new SelectionPathController(this);
        }
        void OnDisable()
        {
            _pathController?.Dispose();
        }

        void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            
            DisplayOptions();
            
            var commonScriptTypes = GetFilteredScriptTypesWithNamespace(_scriptTypeSearch, "Common");
            var customScriptTypes = GetFilteredScriptTypes(_scriptTypeSearch);
            
            GUILayout.Space(10);
            DisplayCommonTypes();
            
            GUILayout.Space(10);
            DisplayCustomTypes();
            
            EditorGUILayout.EndScrollView();

            return;

            void DisplayOptions()
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Type:", GUILayout.Width(50));
                SOType newSOType = (SOType)EditorGUILayout.EnumPopup(_selectedSOType, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
        
                if (newSOType != _selectedSOType)
                {
                    _selectedSOType = newSOType;
                    InitializeScriptTypes();
                }

                GUILayout.BeginHorizontal();
                GUILayout.Label("Filter:", GUILayout.Width(50));
                _scriptTypeSearch = EditorGUILayout.TextField(_scriptTypeSearch, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
            
                GUILayout.BeginHorizontal();
                GUILayout.Label("Name:", GUILayout.Width(50));
                _assetName = EditorGUILayout.TextField(_assetName, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
            
                _pathController.DisplayPathGUI();
            }

            void DisplayCommonTypes()
            {
                if (commonScriptTypes.Length <= 0) return;
                
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Common", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                DrawHorizontalLine(2);
        
                foreach (var scriptType in commonScriptTypes)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(scriptType);
                    DisplayCreateButton(scriptType);
                    GUILayout.EndHorizontal();
                    DrawHorizontalLine();
                }
            }

            void DisplayCustomTypes()
            {
                if (customScriptTypes.Length <= 0) return;
                
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Custom", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                DrawHorizontalLine(2);
        
                foreach (var scriptType in customScriptTypes)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(scriptType);
                    DisplayCreateButton(scriptType);
                    GUILayout.EndHorizontal();
                    DrawHorizontalLine();
                }
            }

            void DisplayCreateButton(string scriptType)
            {
                GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    normal = { textColor = new Color(1, 0.3f, 0f, 1f) },
                    fixedWidth = 75,
                    fixedHeight = 25
                };
                
                Color ogColor = GUI.backgroundColor;
                GUI.backgroundColor = new Color(0.6f, 0.9f, 1f, 1f);
                if (GUILayout.Button("Create", buttonStyle))
                {
                    if (!_pathController.EmptyPath) CreateScriptableObjectInstance(scriptType);
                    else Debug.LogWarning("Invalid path: Please select a folder in the project panel");
                }
                GUI.backgroundColor = ogColor;
            }
            
            void DrawHorizontalLine(float height = 1)
            {
                Rect rect = EditorGUILayout.GetControlRect(false, height);
                EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
            }
        }

        string[] GetFilteredScriptTypes(string search)
        {
            return _scriptTypes
                .Where(type => type.Namespace == null || !type.Namespace.ToLower().Contains("common") &&
                    type.Name.ToLower().Contains(search.ToLower()))
                .Select(type => TrimScriptType(type.Name))
                .ToArray();
        }

        string[] GetFilteredScriptTypesWithNamespace(string search, string namespaceFilter)
        {
            return _scriptTypes
                .Where(type => type.Namespace != null && type.Namespace.ToLower().Contains(namespaceFilter.ToLower()) &&
                               type.Name.ToLower().Contains(search.ToLower()))
                .Select(type => TrimScriptType(type.Name))
                .ToArray();
        }

        string TrimScriptType(string typeName)
        {
            return _selectedSOType switch
            {
                SOType.Variable => typeName.EndsWith("VariableSO") ? typeName[..^10] : typeName,
                SOType.ListVariable => typeName.EndsWith("ListVariableSO") ? typeName[..^14] : typeName,
                SOType.RuntimeSet => typeName.EndsWith("RuntimeSetSO") ? typeName[..^12] : typeName,
                SOType.RuntimeSingle => typeName.EndsWith("RuntimeSingleSO") ? typeName[..^15] : typeName,
                _ => typeName
            };
        }

        void CreateScriptableObjectInstance(string typeName)
        {
            Type selectedType = _scriptTypes.First(type => TrimScriptType(type.Name) == typeName);

            ScriptableObject instance = CreateInstance(selectedType);

            string suffix = _selectedSOType switch
            {
                SOType.RuntimeSet => "RuntimeSet",
                SOType.RuntimeSingle => "RuntimeSingle",
                SOType.Variable => "Variable",
                SOType.ListVariable => "ListVariable",
                _ => "Type Not Implemented"
            };

            string baseAssetName = string.IsNullOrEmpty(_assetName)
                ? $"New {TrimScriptType(selectedType.Name)} {suffix}"
                : _assetName;

            string assetPath = $"{_pathController.SelectionPath}/{baseAssetName}.asset";
            string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            AssetDatabase.CreateAsset(instance, uniqueAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = instance;
        }

        void InitializeScriptTypes()
        {
            switch (_selectedSOType)
            {
                case SOType.Variable:
                    InitializeScriptTypes(typeof(ScriptableVariableBaseSO<>));
                    break;
                case SOType.ListVariable:
                    InitializeScriptTypes(typeof(ScriptableListVariableBaseSO<>));
                    break;
                case SOType.RuntimeSet:
                    InitializeScriptTypes(typeof(RuntimeSetBaseSO<>));
                    break;
                case SOType.RuntimeSingle:
                    InitializeScriptTypes(typeof(RuntimeSingleBaseSO<>));
                    break;
            }
        }

        void InitializeScriptTypes(Type baseType)
        {
            _scriptTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => IsSubclassOfRawGeneric(type, baseType) && !type.IsAbstract && type != baseType)
                .ToArray();
        }

        static bool IsSubclassOfRawGeneric(Type toCheck, Type generic)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                    return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
#endif
}
