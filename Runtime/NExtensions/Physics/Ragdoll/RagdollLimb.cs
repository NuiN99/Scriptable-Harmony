using UnityEngine;

namespace NuiN.NExtensions
{
    public class RagdollLimb : MonoBehaviour
    {
        public Ragdoll ragdoll;
        public Rigidbody rb;
        public Collider col;

        void Awake()
        {
            col = GetComponent<Collider>();
            rb = GetComponent<Rigidbody>();
        }

        void OnCollisionEnter(Collision other)
        {
            if (!ragdoll.ragdollsOnCollision) return;
            if (ragdoll.ragdolling) return;
            if (!ragdoll.AffectedByLayer(other.gameObject)) return;

            ragdoll.EnableRagdoll();

            Vector3 dir = (other.transform.position - other.GetContact(0).point).normalized;
            rb.AddForce(dir * (other.relativeVelocity.magnitude * ragdoll.limbForceMult), ForceMode.Impulse);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!ragdoll.ragdollsOnCollision) return;
            if (!ragdoll.usesTriggers) return;
            if (ragdoll.ragdolling) return;
            if (!ragdoll.AffectedByLayer(other.gameObject)) return;

            ragdoll.EnableRagdoll();

            if (!other.TryGetComponent(out Rigidbody otherRB)) return;

            Vector3 dir = (other.transform.position - transform.position).normalized;
            rb.AddForce(dir * (otherRB.velocity.magnitude * ragdoll.limbForceMult), ForceMode.Impulse);
        }
    }
}