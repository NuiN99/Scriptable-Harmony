#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using NuiN.ScriptableHarmony.Variable.Base;
using Object = UnityEngine.Object;

namespace NuiN.ScriptableHarmony.Core.Editor.Tools
{
    internal class FindScriptableObjectWindow : EditorWindow
    {
        List<Object> _foundObjects = new();
        Vector2 _scrollPosition;
        static string _typeName;

        int _resultCount;

        string TypeName
        {
            get => _typeName;
            set
            {
                string ogValue = _typeName;
                _typeName = value;
                
                if(_typeName != ogValue) FindObjects();
            }
        }
        
        static SerializedProperty _property;
        string _searchFilter;
        static FindScriptableObjectWindow _windowInstance;

        [MenuItem("ScriptableHarmony/Find a Scriptable Object")]
        static void OpenFindWindowMenuItem() => OpenFindWindow(string.Empty, null);
        
        public static void OpenFindWindow(string typeName, SerializedProperty property)
        {
            _property = property;
            _typeName = typeName;
            _windowInstance = GetWindow<FindScriptableObjectWindow>("Scriptable Object Finder");

            _windowInstance.FindObjects();
        }

        void OnEnable()
        {
            FindObjects();
            EditorApplication.update += CloseIfNotFocused;
        }

        void OnDestroy() => EditorApplication.update -= CloseIfNotFocused;

        void OnGUI()
        {
            DrawTypeSearchBar();
            DrawSearchBar();
            
            DrawTopBar();
            
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            
            DrawSearchResults();
            
            if (_resultCount == 0)
            {
                DrawNoResults();
                return;
            }
            
            EditorGUILayout.EndScrollView();
            return;

            void DrawSearchBar()
            {
                EditorGUILayout.BeginHorizontal();

                GUIContent searchIcon = EditorGUIUtility.IconContent("Search Icon");
                GUILayout.Space(5);
                EditorGUILayout.LabelField(searchIcon, GUILayout.Width(20));
                EditorGUILayout.LabelField("Filter:", GUILayout.Width(35));
                _searchFilter = EditorGUILayout.TextField(_searchFilter);

                EditorGUILayout.EndHorizontal();
            }
            
            void DrawTypeSearchBar()
            {
                EditorGUILayout.BeginHorizontal();
                GUIContent searchIcon = EditorGUIUtility.IconContent("Search Icon");
                GUILayout.Space(5);
                EditorGUILayout.LabelField(searchIcon, GUILayout.Width(20));
                EditorGUILayout.LabelField("Type:", GUILayout.Width(35));
                TypeName = EditorGUILayout.TextField(TypeName);
                
                EditorGUILayout.EndHorizontal();
            }

            void DrawTopBar()
            {
                GUILayout.BeginHorizontal();
            
                EditorGUILayout.LabelField($"Search Results: {_resultCount}");

                if (_property != null && _property.objectReferenceValue != null)
                {
                    GUIStyle emptyFieldButtonStyle = new GUIStyle(GUI.skin.button) { normal = { textColor = Color.white } };
                    Color ogColor = GUI.color;
                    GUI.color = Color.red;
            
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Remove", emptyFieldButtonStyle, GUILayout.Width(60)) && _property != null)
                    {
                        _property.objectReferenceValue = null;
                        _property.serializedObject.ApplyModifiedProperties();
                        Close();
                    }
                    GUI.color = ogColor;
                }
                
                GUILayout.EndHorizontal();
            }

            void DrawNoResults()
            {
                Rect messageRect = new Rect(0, position.height / 2 - 50, position.width, 60);
                GUIStyle messageStyle = new GUIStyle(EditorStyles.label)
                {
                    alignment = TextAnchor.UpperCenter,
                    fontSize = 14,
                    fontStyle = FontStyle.Bold
                };
                string noResultsMessage =
                    _typeName != string.Empty 
                        ? $"No {_typeName} Objects Found" 
                        : "No Type Specified";
                
                EditorGUI.LabelField(messageRect, noResultsMessage, messageStyle);
                EditorGUILayout.EndScrollView();
            }

            void DrawSearchResults()
            {
                _resultCount = 0;
                foreach (Object obj in _foundObjects.Where(obj => string.IsNullOrEmpty(_searchFilter) || obj.name.Contains(_searchFilter, StringComparison.OrdinalIgnoreCase)))
                {
                    _resultCount++;
                    EditorGUILayout.BeginHorizontal();

                    Rect labelRect = GUILayoutUtility.GetRect(new GUIContent(obj.name), EditorStyles.label);
                    Rect objectFieldRect = new Rect(labelRect.x, labelRect.y, labelRect.width, EditorGUIUtility.singleLineHeight);
    
                    if (Event.current.type == EventType.MouseDown && objectFieldRect.Contains(Event.current.mousePosition))
                    {
                        EditorGUIUtility.PingObject(obj);
                        Event.current.Use();
                    }
                
                    EditorGUI.ObjectField(objectFieldRect, GUIContent.none, obj, typeof(ScriptableVariableBaseSO<>), true);
                    GUIStyle style = new GUIStyle(GUI.skin.button) { normal = { textColor = Color.black } };

                    if (_property != null)
                    {
                        if (GUILayout.Button("Assign", style, GUILayout.Width(60)) && _property != null)
                        {
                            _property.objectReferenceValue = obj;
                            _property.serializedObject.ApplyModifiedProperties();
                            Close();
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        void FindObjects()
        {
            _foundObjects.Clear();

            string[] guids = AssetDatabase.FindAssets($"t:{_typeName}");

            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Object obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                if (obj != null && obj.GetType().Name == _typeName) _foundObjects.Add(obj);
            }
        }

        void CloseIfNotFocused()
        {
            if (focusedWindow != _windowInstance) Close();
        }
    }
}

#endif
