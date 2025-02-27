#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace NuiN.NExtensions.Editor
{
    internal static class CenterToChildrenEditorTool
    {
        [MenuItem("CONTEXT/Transform/Center To Children", false, 1500)]
        static void CenterToChildren(MenuCommand command)
        {
            Transform selectedTransform = (Transform)command.context;

            if (selectedTransform.childCount == 0)
            {
                Debug.LogWarning("Selected Transform has no children!");
                return;
            }

            Undo.RecordObject(selectedTransform, "Move Transform to Children Center");

            foreach (Transform child in selectedTransform)
            {
                Undo.RecordObject(child, "Move Child to Maintain Position");
            }

            Vector3 averageCenter = Vector3.zero;
            foreach (Transform child in selectedTransform)
            {
                averageCenter += child.position;
            }

            averageCenter /= selectedTransform.childCount;

            Vector3 originalPosition = selectedTransform.position;
            selectedTransform.position = averageCenter;

            foreach (Transform child in selectedTransform)
            {
                child.position -= (averageCenter - originalPosition);
            }

            Debug.Log("Selected transform moved to the average center of its children.");
        }
    }
}
#endif