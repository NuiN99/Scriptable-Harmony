#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NuiN.NExtensions.Editor
{
    internal class SceneLoaderWindow : EditorWindow
    {
        [MenuItem("Window/Scene Loader", priority = -100)]
        public static void ShowWindow()
        {
            GetWindow<SceneLoaderWindow>("Scene Loader");
        }

        Vector2 _scrollPosition;
        void OnGUI()
        {
            Color buttonTextColor = Application.isPlaying ? Color.red : Color.green;

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));

            foreach ((string name, string path) scene in GetSceneNames())
            {
                GUILayout.BeginHorizontal();

                GUIStyle labelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft };
                GUILayout.Label(scene.name, labelStyle, GUILayout.ExpandWidth(true));

                GUILayout.FlexibleSpace();

                GUIStyle buttonStyle = new GUIStyle(GUI.skin.button) { fixedWidth = 50, normal = { textColor = buttonTextColor } };
                if (GUILayout.Button("Load", buttonStyle)) LoadScene(scene.name, scene.path);

                GUILayout.EndHorizontal();

                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            }

            GUILayout.EndScrollView();
        }


        static IEnumerable<(string name, string path)> GetSceneNames()
        {
            if (Application.isPlaying)
            {
                var scenes = new List<(string name, string path)>();
                for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    string path = SceneUtility.GetScenePathByBuildIndex(i);
                    string name = Path.GetFileNameWithoutExtension(path);
                    scenes.Add((name, path));
                }

                return scenes;
            }
            
            string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" });

            List<(string name, string path)> sceneNames = new();
            foreach (string sceneGuid in sceneGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(sceneGuid);
                string name = Path.GetFileNameWithoutExtension(path);
                sceneNames.Add((name, path));
            }

            return sceneNames;
        }

        static void LoadScene(string sceneName, string scenePath)
        {
            if (Application.isPlaying)
            {
                SceneManager.LoadScene(sceneName);
                return;
            }
            
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}
#endif