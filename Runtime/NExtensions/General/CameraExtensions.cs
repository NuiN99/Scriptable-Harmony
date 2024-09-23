using UnityEngine;

namespace NuiN.NExtensions
{
    public static class CameraExtensions
    {
        public static bool IsInFrustum(this Camera camera, Vector3 point)
        {
            Vector3 screenPos = camera.WorldToScreenPoint(point);
            return screenPos.z > 0;
        }
    }
}
