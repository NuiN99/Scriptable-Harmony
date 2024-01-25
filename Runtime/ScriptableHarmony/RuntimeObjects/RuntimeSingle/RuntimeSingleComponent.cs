using NuiN.ScriptableHarmony.Internal.Helpers;
using NuiN.ScriptableHarmony.References;
using UnityEngine;

namespace NuiN.ScriptableHarmony.RuntimeSingle.Components.Base
{
    public class RuntimeSingleComponent<T> : MonoBehaviour
    {
        [SerializeField] T thisObject;
    
        [SerializeField] SetRuntimeSingle<T> runtimeSingle;
        [SerializeField] LifetimeType lifetimeType;

        [SerializeField] bool overwriteExisting;
        
        [Header("Actions")]
        [SerializeField] bool dontInvokeOnSet;
        [SerializeField] bool dontInvokeOnRemove;
    
        void Reset() => thisObject ??= GetComponent<T>();
        
        void OnEnable() => SetItem(LifetimeType.OnEnableOnDisable);
        void OnDisable()
        {
            if(lifetimeType == LifetimeType.OnlyRemoveOnDestroyAndDisable) RemoveFromSet();
            else RemoveFromSetCondition(LifetimeType.OnEnableOnDisable);
        }

        void Awake()
        {
            thisObject ??= GetComponent<T>();
            SetItem(LifetimeType.OnAwakeOnDestroy);
        }

        void OnDestroy() => RemoveFromSet();

        void SetItem(LifetimeType type)
        {
            if (SelfDestructIfNullObject(thisObject)) return;
            if (lifetimeType != type) return;
            
            switch (dontInvokeOnSet)
            {
                case false when overwriteExisting: runtimeSingle.Set(thisObject); break;
                case false when !overwriteExisting: runtimeSingle.TrySet(thisObject); break;
                case true when overwriteExisting: runtimeSingle.SetNoInvoke(thisObject); break;
                case true when !overwriteExisting: runtimeSingle.TrySetNoInvoke(thisObject); break;
            }
        }
        void RemoveFromSetCondition(LifetimeType type)
        {
            if (lifetimeType == type) RemoveFromSet();
        }
        void RemoveFromSet()
        {
            if (!dontInvokeOnRemove) runtimeSingle.Remove();
            else runtimeSingle.RemoveNoInvoke();
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

