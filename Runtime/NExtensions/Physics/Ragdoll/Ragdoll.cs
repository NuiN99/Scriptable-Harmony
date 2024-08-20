using System.Collections;
using UnityEngine;

namespace NuiN.NExtensions
{
    [SelectionBase]
    public class Ragdoll : MonoBehaviour
    {
        [Header("Setup Options")]
        [SerializeField] float setupTotalMass;
        
        [Header("Values")]
        [SerializeField] float ragdollTime = 3f;
        public float limbForceMult = 2f;
        [SerializeField] bool destroyAfterRagdollTime;

        [Header("Ragdoll Collision Activation")]
        public bool ragdollsOnCollision = true;
        public LayerMask affectedBy;

        [Header("Debugging")]
        [SerializeField] [ReadOnly] CollisionDetectionMode collisionMode;
        [SerializeField] [ReadOnly] float totalMass;
        [SerializeField] [ReadOnly] float limbMass;
        [SerializeField] [ReadOnly] public bool usesTriggers;
        [SerializeField] [ReadOnlyPlayMode] public bool ragdolling;

        Animator _animator;
        RagdollLimb[] _limbs;
        IRagdoll[] _ragdollInterfaces;

        void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _limbs = GetComponentsInChildren<RagdollLimb>();
            _ragdollInterfaces = GetComponentsInChildren<IRagdoll>();

            if (_limbs.Length <= 0)
            {
                Debug.LogError("There are no RagdollLimbs on this Ragdoll: Remember to create the Ragdoll using the Create Ragdoll Button");
            }
        }
        
        [MethodButton("Create Ragdoll")]
        public void SetupRagdollInInspector()
        {
            UpdateLimbComponents();
            SetTotalMass(setupTotalMass);
            SetCollisionDetectionMode(CollisionDetectionMode.Continuous);
            SetColliderTypes(false);
        }

        void UpdateLimbComponents()
        {
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rigidbodies)
            {
                if (rb.TryGetComponent(out RagdollLimb existingLimb))
                {
                    existingLimb.Setup(this);
                }
                else
                {
                    if (rb.gameObject == this.gameObject) continue;
                    RagdollLimb limb = rb.gameObject.AddComponent<RagdollLimb>();
                    limb.Setup(this);
                }
            }
        }
        
        public void SetCollisionDetectionMode(CollisionDetectionMode mode)
        {
            collisionMode = mode;
            foreach (var rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.collisionDetectionMode = mode;
            }
        }
        public void SetTotalMass(float mass)
        {
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            int count = rigidbodies.Length;
            float dividedMass = mass / count;

            totalMass = mass;
            limbMass = dividedMass;

            foreach (var rb in rigidbodies)
            {
                rb.mass = dividedMass;
            }
        }

        public void SetColliderTypes(bool isTrigger)
        {
            foreach (var limb in GetComponentsInChildren<RagdollLimb>())
            {
                limb.GetComponent<Collider>().isTrigger = isTrigger;
            }
        }

        public void ChangeBodyType(CollisionDetectionMode type)
        {
            foreach (var limb in _limbs)
            {
                limb.rb.collisionDetectionMode = type;
            }
        }

        public void EnableRagdoll()
        {
            if (ragdolling) return;
            InitializeRagdoll();

            foreach (var limb in _limbs)
            {
                if (usesTriggers) limb.col.isTrigger = false;

                limb.rb.isKinematic = false;
            }
        }

        public void ThrowRagdoll(Vector3 direction, float force)
        {
            InitializeRagdoll();

            foreach (var limb in _limbs)
            {
                if (usesTriggers) limb.col.isTrigger = false;

                limb.rb.isKinematic = false;
                limb.rb.AddForce(direction * force, ForceMode.Impulse);
            }
        }

        public void DisableRagdoll()
        {
            ragdolling = false;

            foreach(var ragdollInterface in _ragdollInterfaces) ragdollInterface?.RagdollDisabled();

            ToggleAnimator(true);
            foreach (var limb in _limbs)
            {
                if (usesTriggers) limb.col.isTrigger = true;

                if(limb.rb != null) limb.rb.isKinematic = true;
            }
        }

        void ToggleAnimator(bool state)
        {
            if (_animator == null) return;
            _animator.enabled = state;
        }

        void InitializeRagdoll()
        {
            ragdolling = true;
            foreach(var ragdollInterface in _ragdollInterfaces) ragdollInterface?.RagdollEnabled();
            ToggleAnimator(false);
            StartCoroutine(UnRagdollAfterDuration());
        }

        IEnumerator UnRagdollAfterDuration()
        {
            yield return new WaitForSeconds(ragdollTime);
            if (destroyAfterRagdollTime)
            {
                Destroy(gameObject);
                yield break;
            }
            DisableRagdoll();
        }

        public bool AffectedByLayer(GameObject obj)
        {
            return affectedBy.ContainsLayer(obj);
        }
    }
}


