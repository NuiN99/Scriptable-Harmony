using UnityEngine;

namespace NuiN.NExtensions
{
    public static class CollisionExtensions
    {
        public static float TotalCollisionForce(this Collision2D collision)
        {
            Vector2 impulse = Vector2.zero;

            int contactCount = collision.contactCount;
            for (int i = 0; i < contactCount; i++)
            {
                var contact = collision.GetContact(i);
                impulse += contact.normal * contact.normalImpulse;
                impulse.x += contact.tangentImpulse * contact.normal.y;
                impulse.y -= contact.tangentImpulse * contact.normal.x;
            }

            float totalImpulse = Mathf.Abs(impulse.x) + Mathf.Abs(impulse.y);
            return totalImpulse;
        }
    }
}

