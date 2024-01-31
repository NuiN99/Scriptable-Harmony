#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NuiN.NExtensions
{
    internal class SceneLoaderWindow : EditorWindow
    {
        [MenuItem("Window/Scene Loader", priority = -100)]
        public static void ShowWindow()
        {
            GetWindow<SceneLoaderWindow>("Scene Loader");
        }

        void OnGUI()
        {
            IEnumerable<string> sceneNames = GetSceneNames();

            foreach (string sceneName in sceneNames)
            {
                GUILayout.BeginHorizontal();

                GUIStyle labelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft };
                GUILayout.Label(sceneName, labelStyle, GUILayout.ExpandWidth(true));

                GUIStyle buttonStyle = new GUIStyle(GUI.skin.button) { fixedWidth = 50, normal = { textColor = Color.green } };
                if (GUILayout.Button("Load", buttonStyle)) LoadScene(sceneName);

                GUILayout.EndHorizontal();

                // Add a divider between scenes
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            }
        }

        static IEnumerable<string> GetSceneNames()
        {
            const string scenesDirectory = "Assets/Scenes/";
            string[] sceneFiles = System.IO.Directory.GetFiles(scenesDirectory, "*.unity");
            return sceneFiles.Select(System.IO.Path.GetFileNameWithoutExtension).ToList();
        }

        static void LoadScene(string sceneName)
        {
            if (Application.isPlaying)
            {
                SceneManager.LoadScene(sceneName);
                return;
            }
            
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            string scenePath = "Assets/Scenes/" + sceneName + ".unity";
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}
#endif