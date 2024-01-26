using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    public class RuntimeSingleComponent<T> : MonoBehaviour
    {
        [SerializeField] T thisObject;
    
        [SerializeField] SetRuntimeSingle<T> runtimeSingle;
        [SerializeField] LifetimeType lifetimeType;

        [SerializeField] bool overwriteExisting;
        
        void Reset() => thisObject ??= GetComponent<T>();
        
        void OnEnable() => SetItem(LifetimeType.OnEnableOnDisable);
        void OnDisable()
        {
            if(lifetimeType == LifetimeType.RemoveOnDestroyAndDisable) runtimeSingle.Remove();
            else RemoveFromSetCondition(LifetimeType.OnEnableOnDisable);
        }

        void Awake()
        {
            thisObject ??= GetComponent<T>();
            SetItem(LifetimeType.OnAwakeOnDestroy);
        }

        void OnDestroy() => runtimeSingle.Remove();

        void SetItem(LifetimeType type)
        {
            if (SelfDestructIfNullObject(thisObject)) return;
            if (lifetimeType != type) return;
            
            if(overwriteExisting) runtimeSingle.Set(thisObject);
            else runtimeSingle.TrySet(thisObject);
        }
        void RemoveFromSetCondition(LifetimeType type)
        {
            if (lifetimeType == type) runtimeSingle.Remove();
        }

        bool SelfDestructIfNullObject(T obj)
        {
            if (obj != null) return false;
        
            Debug.LogError($"Self Destructing: {typeof(T).Name} object not assigned in {gameObject.name}'s RuntimeSingle Component", gameObject);
            Destroy(this);
            return true;
        }
    }
}

