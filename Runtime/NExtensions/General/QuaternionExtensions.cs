using UnityEngine;

namespace NuiN.NExtensions
{
    public static class QuaternionExtensions
    {
        public static Quaternion With(this Quaternion q, float? x = null, float? y = null, float? z = null) =>
            Quaternion.Euler(x ?? q.x, y ?? q.y, z ?? q.z);
        
        public static Quaternion Add(this Quaternion q, float x = 0, float y = 0, float z = 0) =>
            Quaternion.Euler(q.x + x, q.y + y, q.z + z);
    }
}