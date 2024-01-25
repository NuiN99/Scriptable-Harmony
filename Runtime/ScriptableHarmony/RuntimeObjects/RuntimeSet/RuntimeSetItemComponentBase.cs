using NuiN.ScriptableHarmony.Internal.Helpers;
using NuiN.ScriptableHarmony.References;
using UnityEngine;

namespace NuiN.ScriptableHarmony.RuntimeSet.Components.Base
{
    public class RuntimeSetItemComponentBase<T> : MonoBehaviour
    {
        [SerializeField] T thisObject;
    
        [SerializeField] SetRuntimeSet<T> runtimeSet;
        [SerializeField] LifetimeType lifetimeType;
        
        [Header("Actions")]
        [SerializeField] bool dontInvokeOnAdd;
        [SerializeField] bool dontInvokeOnRemove;

        void Reset() => thisObject ??= GetComponent<T>();

        void OnEnable() => AddToSet(LifetimeType.OnEnableOnDisable);
        void OnDisable()
        {
            if(lifetimeType == LifetimeType.OnlyRemoveOnDestroyAndDisable) RemoveFromSet();
            else RemoveFromSetCondition(LifetimeType.OnEnableOnDisable);
        }

        void Awake()
        {
            thisObject ??= GetComponent<T>();
            AddToSet(LifetimeType.OnAwakeOnDestroy);
        }

        void OnDestroy()
        {
            RemoveFromSet();
        }

        void AddToSet(LifetimeType type)
        {
            if (SelfDestructIfNullObject(thisObject)) return;
            if (lifetimeType != type) return;
            
            if(!dontInvokeOnAdd) runtimeSet.Add(thisObject);
            else runtimeSet.AddNoInvoke(thisObject);
        }
        void RemoveFromSetCondition(LifetimeType type)
        {
            if (lifetimeType == type) RemoveFromSet();
        }
        void RemoveFromSet()
        {
            if(!dontInvokeOnRemove) runtimeSet.Remove(thisObject);
            else runtimeSet.RemoveNoInvoke(thisObject);
        }

        bool SelfDestructIfNullObject(T obj)
        {
            if (obj != null) return false;
        
            Debug.LogError($"Self Destructing: {typeof(T).Name} object not assigned in {gameObject.name}'s RuntimeSet Component", gameObject);
            Destroy(this);
            return true;
        }
    }
}

