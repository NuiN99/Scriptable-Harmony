using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using NuiN.ScriptableHarmony.Core;

namespace NuiN.ScriptableHarmony.Editor
{
#if UNITY_EDITOR
    internal class CreateScriptableObjectGUI
    {
        Type[] _scriptTypes = Type.EmptyTypes;
        SOType _selectedSOType = SOType.Variable;
        string _scriptTypeSearch = "";
        string _assetName;
        Vector2 _scrollPosition;

        bool _initalized;
        
        SelectionPathController _pathController;

        public CreateScriptableObjectGUI(SelectionPathController pathController)
        {
            _pathController = pathController;
        }

        public void DrawGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            
            DisplayOptions();
            
            var commonScriptTypes = GetFilteredScriptTypesWithNamespace(_scriptTypeSearch);
            
            GUILayout.Space(10);
            
            DrawHeader();
            DisplayTypes();
            
            EditorGUILayout.EndScrollView();

            return;

            void DisplayOptions()
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Type:", GUILayout.Width(50));
                SOType newSOType = (SOType)EditorGUILayout.EnumPopup(_selectedSOType, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
        
                if (newSOType != _selectedSOType || !_initalized)
                {
                    _initalized = true;
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

            void DrawHeader()
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(_selectedSOType.ToString(), EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                DrawHorizontalLine(2);
            }

            void DisplayTypes()
            {
                if (commonScriptTypes.Length <= 0) return;
        
                foreach (var scriptType in commonScriptTypes)
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

        string[] GetFilteredScriptTypesWithNamespace(string search, string namespaceFilter = "")
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
                SOType.List => typeName.EndsWith("ListSO") ? typeName[..^6] : typeName,
                SOType.RuntimeSet => typeName.EndsWith("RuntimeSetSO") ? typeName[..^12] : typeName,
                SOType.RuntimeSingle => typeName.EndsWith("RuntimeSingleSO") ? typeName[..^15] : typeName,
                SOType.Dictionary => typeName.EndsWith("DictionarySO") ? typeName[..^10] : typeName,
                SOType.Variable => typeName.EndsWith("SO") ? typeName[..^2] : typeName,
                _ => typeName
            };
        }

        void CreateScriptableObjectInstance(string typeName)
        {
            Type selectedType = _scriptTypes.First(type => TrimScriptType(type.Name) == typeName);

            ScriptableObject instance = ScriptableObject.CreateInstance(selectedType);

            string suffix = _selectedSOType switch
            {
                SOType.RuntimeSet => "RuntimeSet",
                SOType.RuntimeSingle => "RuntimeSingle",
                SOType.Variable => "",
                SOType.List => "List",
                SOType.Dictionary => "Dictionary",
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
                    InitializeScriptTypes(typeof(ScriptableVariableSO<>));
                    break;
                case SOType.List:
                    InitializeScriptTypes(typeof(ScriptableListSO<>));
                    break;
                case SOType.RuntimeSet:
                    InitializeScriptTypes(typeof(RuntimeSetSO<>));
                    break;
                case SOType.RuntimeSingle:
                    InitializeScriptTypes(typeof(RuntimeSingleSO<>));
                    break;
                case SOType.Dictionary:
                    InitializeScriptTypes(typeof(ScriptableDictionarySO<,>));
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
