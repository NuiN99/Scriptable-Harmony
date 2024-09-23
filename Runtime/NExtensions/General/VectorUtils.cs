using UnityEngine;

namespace NuiN.NExtensions
{
    public static class VectorUtils
    {
        public static Vector3 Direction(Vector3 start, Vector3 end) 
            => (end - start).normalized;
        
        public static float DirectionAngle(Vector3 direction) 
            => Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        public static Vector2 AngleDirection2D(float angle)
        {
            float radians = Mathf.Deg2Rad * angle;
            return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        }
    }
}

