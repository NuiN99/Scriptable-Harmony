#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using NuiN.ScriptableHarmony.Core;
using Object = UnityEngine.Object;

namespace NuiN.ScriptableHarmony.Editor
{
    internal class FindScriptableObjectGUI
    {
        List<Object> _foundObjects = new();
        Vector2 _scrollPosition;
        static string typeName;
        SOType _soType;

        int _resultCount;

        string TypeName
        {
            get => typeName;
            set
            {
                string ogValue = typeName;
                typeName = value;
                
                if(typeName != ogValue) FindObjects();
            }
        }
        
        static SerializedProperty property;
        string _searchFilter;

        public bool openedFromField;

        public FindScriptableObjectGUI(string typeName, SerializedProperty property, bool openedFromField, SOType soType)
        {
            this.openedFromField = openedFromField;
            
            FindScriptableObjectGUI.property = property;
            FindScriptableObjectGUI.typeName = typeName;

            FindObjects();
        }

        public void DrawGUI(EditorWindow window)
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

                if (property != null && property.objectReferenceValue != null)
                {
                    GUIStyle emptyFieldButtonStyle = new GUIStyle(GUI.skin.button) { normal = { textColor = Color.white } };
                    Color ogColor = GUI.color;
                    GUI.color = Color.red;
            
                    GUILayout.FlexibleSpace();
                    if (openedFromField && GUILayout.Button("Remove", emptyFieldButtonStyle, GUILayout.Width(60)) && property != null)
                    {
                        property.objectReferenceValue = null;
                        property.serializedObject.ApplyModifiedProperties();
                        window.Close();
                    }
                    GUI.color = ogColor;
                }
                
                GUILayout.EndHorizontal();
            }

            void DrawNoResults()
            {
                Rect messageRect = new Rect(0, window.position.height / 2 - 50, window.position.width, 60);
                GUIStyle messageStyle = new GUIStyle(EditorStyles.label)
                {
                    alignment = TextAnchor.UpperCenter,
                    fontSize = 14,
                    fontStyle = FontStyle.Bold
                };
                string noResultsMessage =
                    typeName != string.Empty 
                        ? $"No {typeName} Objects Found" 
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
                
                    EditorGUI.ObjectField(objectFieldRect, GUIContent.none, obj, typeof(ScriptableVariableSO<>), true);
                    GUIStyle style = new GUIStyle(GUI.skin.button) { normal = { textColor = Color.black } };

                    if (property != null)
                    {
                        if (openedFromField && GUILayout.Button("Assign", style, GUILayout.Width(60)) && property != null)
                        {
                            property.objectReferenceValue = obj;
                            property.serializedObject.ApplyModifiedProperties();
                            window.Close();
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        void FindObjects()
        {
            _foundObjects.Clear();

            string[] guids = AssetDatabase.FindAssets($"t:{typeName}");

            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Object obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                if (obj != null && obj.GetType().Name == typeName) _foundObjects.Add(obj);
            }
        }
    }
}

#endif
