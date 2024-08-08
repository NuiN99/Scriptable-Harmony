using UnityEngine;
using System;

namespace NuiN.NExtensions.Editor
{
    public static class GizmoUtils
    {
        public static void DrawCircle(Vector2 position, float radius, Color color, int resolution = 25)
        {
            var points = new Vector2[resolution];
            
            float angle = default;
            float angleIncrement = 360f / resolution;

            for (int i = 0; i < resolution; i++)
            {
                Vector2 point = VectorUtils.AngleDirection2D(angle) * radius;
                points[i] = point;
                angle += angleIncrement;

                if (i == 0) continue;
                
                Vector2 prevPoint = points[i - 1];
                Debug.DrawLine(prevPoint + position, point + position, color);
            }
            
            Debug.DrawLine(points[^1] + position, points[0] + position, color);
        }

        public static void DrawString(string text, int fontSize, Vector3 worldPos, Color? colour = null, Vector2 offset = new()) 
        {
    #if UNITY_EDITOR
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            Camera cam = view == null ? Camera.main : view.camera;
            if (cam == null) return;
            
            UnityEditor.Handles.BeginGUI();

            Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0) {
                UnityEditor.Handles.EndGUI();
                return;
            }

            float distFromCam = Vector3.Distance(worldPos, cam.transform.position);
            int roundedDistFromCam = Mathf.RoundToInt(distFromCam);
            
            // too far to be visible, so don't draw
            if (roundedDistFromCam <= 0)
            {
                return;
            }

            fontSize /= roundedDistFromCam;

            GUIStyle style = new GUIStyle
            {
                fontSize = fontSize,
                normal = { textColor = colour ?? Color.white }
            };
            
            float width = style.CalcSize(new GUIContent(text)).x;
            float widthOffset = width / 2;
            float heightOffset = -(style.CalcHeight(new GUIContent(text), width) / 2);
            Vector2 sizeOffset = new Vector2(widthOffset, heightOffset);
            offset -= sizeOffset;
            
            UnityEditor.Handles.Label(cam ? cam.ScreenToWorldPoint(cam.WorldToScreenPoint(worldPos) + (Vector3)offset) : worldPos, text, style);

            UnityEditor.Handles.EndGUI();
    #endif
        }
    }
}

