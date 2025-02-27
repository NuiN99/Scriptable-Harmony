#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace NuiN.NExtensions.Editor
{
    public class RenameGameObjectsEditorTool : EditorWindow
    {
        string _baseName = "GameObject";
        bool _focusTextField = true;

        [MenuItem("GameObject/Rename Selected Objects", false, 0)]
        static void ShowRenameWindow()
        {
            RenameGameObjectsEditorTool window = GetWindow<RenameGameObjectsEditorTool>(true, "Rename Selected Objects");
            window.minSize = new Vector2(300, 70);
            window.maxSize = new Vector2(300, 70);
        }

        void OnGUI()
        {
            GUILayout.Label("Rename Selected Objects", EditorStyles.boldLabel);

            GUI.SetNextControlName("BaseNameField");
            _baseName = EditorGUILayout.TextField("Base Name", _baseName);

            if (_focusTextField)
            {
                EditorGUI.FocusTextInControl("BaseNameField");
                _focusTextField = false;
            }

            if (GUILayout.Button("Rename Objects"))
            {
                RenameSelectedObjects();
                Close();
            }
        }

        void RenameSelectedObjects()
        {
            GameObject[] selectedObjects = Selection.gameObjects;

            if (selectedObjects.Length == 0)
            {
                EditorUtility.DisplayDialog("Error", "No GameObjects selected.", "OK");
                return;
            }

            System.Array.Sort(selectedObjects, (a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

            Undo.RecordObjects(selectedObjects, "Rename Objects");

            for (int i = 0; i < selectedObjects.Length; i++)
            {
                selectedObjects[i].name = $"{_baseName}_{i}";
            }
        }
    }
}
#endif