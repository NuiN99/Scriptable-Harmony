using NuiN.ScriptableHarmony.References;
using UnityEngine;

namespace NuiN.ScriptableHarmony.RuntimeSingle.Components.Base
{
    public class RuntimeSingleItemComponentBase<T> : MonoBehaviour
    {
        enum Type{ OnEnableOnDisable, OnAwakeOnDestroy }
    
        [SerializeField] T thisObject;
    
        [SerializeField] SetRuntimeSingle<T> runtimeSingle;
        [SerializeField] Type lifetimeType;

        [SerializeField] bool overwriteExisting;
        
        [Header("Actions")]
        [SerializeField] bool dontInvokeOnSet;
        [SerializeField] bool dontInvokeOnRemove;
    
        void Reset() => thisObject ??= GetComponent<T>();
        
        void OnEnable() => SetItem(Type.OnEnableOnDisable);
        void OnDisable() => RemoveFromSet(Type.OnEnableOnDisable);

        void Awake()
        {
            thisObject ??= GetComponent<T>();
            SetItem(Type.OnAwakeOnDestroy);
        }

        void OnDestroy() => RemoveFromSet(lifetimeType);

        void SetItem(Type type)
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
        void RemoveFromSet(Type type)
        {
            if (lifetimeType != type) return;
            
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

