using System;
using UnityEngine;

namespace NuiN.NExtensions
{
    public abstract class State<TEnum, TContext> where TEnum : Enum
    {
        public abstract TEnum StateID { get; }
        public TContext Context { get; protected set; }
        
        public State(TContext context)
        {
            Context = context;
        }

        public abstract TEnum GetNextState();
        
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void FrameUpdate() { }
        public virtual void PhysicsUpdate() { }
    
        public virtual void CollisionEnter(Collision other) { }
        public virtual void CollisionStay(Collision other) { }
        public virtual void CollisionExit(Collision other) { }
        public virtual void TriggerEnter(Collider other) { }
        public virtual void TriggerStay(Collider other) { }
        public virtual void TriggerExit(Collider other) { }
        public virtual void DrawGizmos() { }
    }
}
