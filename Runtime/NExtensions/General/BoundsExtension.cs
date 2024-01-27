using UnityEngine;

namespace NuiN.NExtensions
{
    public static class BoundsExtension
    {
        public static Vector2 RandomPosition2D(this Bounds bounds)
        {
            float maxExtentsX = bounds.extents.x;
            float minExtentsX = -maxExtentsX;

            float maxExtentsY = bounds.extents.y;
            float minExtentsY = -maxExtentsY;

            Vector2 randomPos = new Vector2(
                Random.Range(minExtentsX, maxExtentsX),
                Random.Range(minExtentsY, maxExtentsY));
            randomPos += (Vector2)bounds.center;

            return randomPos;
        }

        public static Vector2 RandomPosition2D(this BoxCollider2D boxCollider)
        {
            return RandomPosition2D(boxCollider.bounds);
        }
    }
}

