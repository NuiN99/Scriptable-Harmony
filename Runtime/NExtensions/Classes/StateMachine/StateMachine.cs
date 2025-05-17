using System;
using System.Collections.Generic;
using UnityEngine;

namespace NuiN.NExtensions
{
    public abstract class StateMachine<TEnum, TContext> : MonoBehaviour where TEnum : Enum
    {
        [SerializeField] protected TContext context;
        [SerializeField] TEnum initialState;
        [SerializeField] bool logTransitions = true;
        
        public State<TEnum, TContext> CurrentState { get; private set; }

        protected abstract State<TEnum, TContext> CreateState(TEnum stateID);

        public void Transition(TEnum newState)
        {
            if (logTransitions && CurrentState != null)
            {
                Debug.Log($"{gameObject.name} | transitioning from {CurrentState.StateID} to {newState}", gameObject);
            }
            
            CurrentState?.Exit();
            CurrentState = CreateState(newState);
            CurrentState.Enter();
        }
        
        void Start()
        {
            Transition(initialState);
        }
        
        void Update()
        {
            if (CurrentState == null) return;

            TEnum nextState = CurrentState.GetNextState();
            
            if (!nextState.Equals(CurrentState.StateID))
            {
                Transition(CurrentState.GetNextState());
            }

            CurrentState.FrameUpdate();
        }

        void FixedUpdate() => CurrentState?.PhysicsUpdate();

        void OnCollisionEnter(Collision other) => CurrentState?.CollisionEnter(other);
        void OnCollisionStay(Collision other) => CurrentState?.CollisionStay(other);
        void OnCollisionExit(Collision other) => CurrentState?.CollisionExit(other);

        void OnTriggerEnter(Collider other) => CurrentState?.TriggerEnter(other);
        void OnTriggerStay(Collider other) => CurrentState?.TriggerStay(other);
        void OnTriggerExit(Collider other) => CurrentState?.TriggerExit(other);
        void OnDrawGizmos() => CurrentState?.DrawGizmos();
    }
}