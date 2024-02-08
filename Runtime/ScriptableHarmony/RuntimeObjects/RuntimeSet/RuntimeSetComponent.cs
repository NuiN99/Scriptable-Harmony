using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    public class RuntimeSetComponent<T> : MonoBehaviour
    {
        [SerializeField] T thisObject;
    
        [SerializeField] SetRuntimeSet<T> runtimeSet;
        [SerializeField] RuntimeSetLifetime lifetimeType;
        
        void Reset() => thisObject ??= GetComponent<T>();

        void OnEnable() => AddToSet(RuntimeSetLifetime.OnEnableOnDisable);
        void OnDisable()
        {
            if(lifetimeType == RuntimeSetLifetime.RemoveOnDestroyAndDisable) RemoveFromSet();
            else RemoveFromSetConditionial(RuntimeSetLifetime.OnEnableOnDisable);
        }

        void Awake()
        {
            thisObject ??= GetComponent<T>();
            AddToSet(RuntimeSetLifetime.OnAwakeOnDestroy);
        }

        void OnDestroy()
        {
            RemoveFromSet();
        }

        void AddToSet(RuntimeSetLifetime type)
        {
            if (SelfDestructIfNullObject(thisObject)) return;
            if (lifetimeType != type) return;
            
            runtimeSet.Add(thisObject);
        }
        void RemoveFromSetConditionial(RuntimeSetLifetime type)
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

