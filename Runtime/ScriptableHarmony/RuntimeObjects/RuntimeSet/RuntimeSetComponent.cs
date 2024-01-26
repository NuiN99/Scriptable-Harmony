using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    public class RuntimeSetComponent<T> : MonoBehaviour
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
            if(lifetimeType == LifetimeType.RemoveOnDestroyAndDisable) RemoveFromSet();
            else RemoveFromSetConditionial(LifetimeType.OnEnableOnDisable);
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
            
            runtimeSet.Add(thisObject);
        }
        void RemoveFromSetConditionial(LifetimeType type)
        {
            if (lifetimeType == type) RemoveFromSet();
        }
        void RemoveFromSet()
        {
            runtimeSet.Remove(thisObject);
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

