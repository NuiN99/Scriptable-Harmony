using NExtensions.Forces;
using UnityEngine;

namespace NuiN.NExtensions
{
    public static class ForcesCreator
    {
        /// <summary>
        /// Adds force from a point outwards, with more force the closer the Rigidbody is to the point
        /// </summary>
        public static void CreateExplosion(Vector3 explosionPos, float radius, float force, LayerMask? affectedLayers = null)
        {
            bool useMask = affectedLayers != null;

            Collider[] allHits = useMask
                ? Physics.OverlapSphere(explosionPos, radius, affectedLayers.Value)
                : Physics.OverlapSphere(explosionPos, radius);

            foreach (Collider col in allHits)
            {
                if (!col.transform.TryGetComponent(out Rigidbody rb)) continue;

                float distFromExplosion = Vector3.Distance(explosionPos, col.transform.position);
                Vector3 forceDir = (col.transform.position - explosionPos).normalized;
                float forceByDistance = (radius - distFromExplosion) * force;
                
                rb.AddForceAtPosition(forceDir * forceByDistance, col.transform.position, ForceMode.Impulse);
            }
        }
        
        /// <summary>
        /// Adds force from a point outwards, with more force the closer the Rigidbody2D is to the point
        /// </summary>
        public static void CreateExplosion2D(Vector2 explosionPos, float radius, float force, LayerMask? affectedLayers = null)
        {
            bool useMask = affectedLayers != null;

            Collider2D[] allHits = useMask
                ? Physics2D.OverlapCircleAll(explosionPos, radius, affectedLayers.Value)
                : Physics2D.OverlapCircleAll(explosionPos, radius);

            foreach (Collider2D col in allHits)
            {
                if (!col.transform.TryGetComponent(out Rigidbody2D rb)) continue;

                float distFromExplosion = Vector3.Distance(explosionPos, col.transform.position);
                Vector2 forceDir = ((Vector2)col.transform.position - explosionPos).normalized;
                float forceByDistance = (radius - distFromExplosion) * force;

                rb.AddForceAtPosition(forceDir * forceByDistance, col.transform.position, ForceMode2D.Impulse);
                
                if(col.TryGetComponent(out IForceAffected forceable)) forceable.OnForceApplied(forceDir, forceByDistance);
            }
        }
    }
}