using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    public class RuntimeSingleComponent<T> : MonoBehaviour
    {
        [SerializeField] T thisObject;
    
        [SerializeField] SetRuntimeSingle<T> runtimeSingle;
        [SerializeField] RuntimeSetLifetime lifetimeType;

        [SerializeField] bool overwriteExisting;
        
        void Reset() => thisObject ??= GetComponent<T>();
        
        void OnEnable() => SetItem(RuntimeSetLifetime.OnEnableOnDisable);
        void OnDisable()
        {
            if(lifetimeType == RuntimeSetLifetime.RemoveOnDestroyAndDisable) runtimeSingle.Remove();
            else RemoveFromSetCondition(RuntimeSetLifetime.OnEnableOnDisable);
        }

        void Awake()
        {
            thisObject ??= GetComponent<T>();
            SetItem(RuntimeSetLifetime.OnAwakeOnDestroy);
        }

        void OnDestroy() => runtimeSingle.Remove();

        void SetItem(RuntimeSetLifetime type)
        {
            if (SelfDestructIfNullObject(thisObject)) return;
            if (lifetimeType != type) return;
            
            if(overwriteExisting) runtimeSingle.Set(thisObject);
            else runtimeSingle.TrySet(thisObject);
        }
        void RemoveFromSetCondition(RuntimeSetLifetime type)
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

