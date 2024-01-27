using UnityEngine;

namespace NuiN.NExtensions
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
    }
}

