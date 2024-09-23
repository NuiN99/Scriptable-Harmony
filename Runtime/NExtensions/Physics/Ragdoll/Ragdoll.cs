using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NuiN.NExtensions
{
    [SelectionBase]
    public class Ragdoll : MonoBehaviour
    {
        public event Action OnRagdollEnabled = delegate { };
        public event Action OnRagdollDisabled = delegate { };
        
        [ShowInInspector] public bool IsRagdolling { get; private set; }
        
        [SerializeField, InjectComponent] Animator animator;
        
        public IReadOnlyList<RagdollLimb> Limbs => _limbs.ToList().AsReadOnly();
        
        [field: SerializeField] public float TotalMass { get; private set; }

        [Header("Debugging")]
        [ShowInInspector] float _totalMass;
        [ShowInInspector] float _individualLimbMass;
        
        RagdollLimb[] _limbs;
        
        void Awake()
        {
            _limbs = GetComponentsInChildren<RagdollLimb>();

            if (_limbs.Length <= 0)
            {
                Debug.LogError("There are no RagdollLimbs on this Ragdoll: Remember to create the Ragdoll using the Create Ragdoll Button");
            }
        }
        
        [MethodButton("Create Ragdoll")]
        public void SetupRagdollInInspector()
        {
            UpdateLimbComponents();
            SetTotalMass(TotalMass);
            SetCollisionDetectionMode(CollisionDetectionMode.ContinuousDynamic);
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

            _totalMass = mass;
            _individualLimbMass = dividedMass;

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
            if (IsRagdolling) return;
            InitializeRagdoll();

            foreach (var limb in _limbs)
            {
                limb.rb.isKinematic = false;
            }
        }

        public void ThrowRagdoll(Vector3 direction, float force)
        {
            InitializeRagdoll();

            foreach (var limb in _limbs)
            {
                limb.rb.isKinematic = false;
                limb.rb.AddForce(direction * force, ForceMode.Impulse);
            }
        }

        public void DisableRagdoll()
        {
            IsRagdolling = false;

            OnRagdollDisabled.Invoke();

            ToggleAnimator(true);
            foreach (var limb in _limbs)
            {
                if(limb.rb != null) limb.rb.isKinematic = true;
            }
        }

        void ToggleAnimator(bool state)
        {
            if (animator == null) return;
            animator.enabled = state;
        }

        void InitializeRagdoll()
        {
            IsRagdolling = true;
            
            OnRagdollEnabled.Invoke();
            
            ToggleAnimator(false);
        }
    }
}


