using UnityEngine;

namespace NuiN.NExtensions
{
    public class RagdollLimb : MonoBehaviour
    {
        public Rigidbody rb;

        public void Setup(Ragdoll ragdoll)
        {
            rb = GetComponent<Rigidbody>();
        }
    }
}