using System;
using UnityEngine;

namespace NuiN.NExtensions
{
    public class CollisionEventDispatcher : MonoBehaviour
    {
        public event Action<Collision> CollisionEnter;
        public event Action<Collision> CollisionExit;
        public event Action<Collision> CollisionStay;
        
        public event Action<Collider> TriggerEnter;
        public event Action<Collider> TriggerExit;
        public event Action<Collider> TriggerStay;
        
        void OnCollisionEnter(Collision other) => CollisionEnter?.Invoke(other);
        void OnCollisionExit(Collision other) => CollisionExit?.Invoke(other);
        void OnCollisionStay(Collision other) => CollisionStay?.Invoke(other);
        void OnTriggerEnter(Collider other) => TriggerEnter?.Invoke(other);
        void OnTriggerExit(Collider other) => TriggerExit?.Invoke(other);
        void OnTriggerStay(Collider other) => TriggerStay?.Invoke(other);
    }
}

