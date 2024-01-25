using System;
using NuiN.ScriptableHarmony.RuntimeSingle.Base;
using UnityEngine;

namespace NuiN.ScriptableHarmony.RuntimeSingle.References.Base
{
    [Serializable]
    public abstract class RuntimeSingleReference<T>
    {
        [SerializeField] protected RuntimeSingleSO<T> runtimeSingle;
        
        public bool IsNull => runtimeSingle == null;
        
        public void SetToResource(string resourceName) => runtimeSingle = Resources.Load<RuntimeSingleSO<T>>(resourceName);
        
        public void SubOnSet(Action<T> onSet) => runtimeSingle.onSet += onSet;
        public void UnSubOnSet(Action<T> onSet) => runtimeSingle.onSet -= onSet;
        
        public void SubOnSetWithOld(Action<T,T> onSetWithOld) => runtimeSingle.onSetWithOld += onSetWithOld;
        public void UnSubOnSetWithOld(Action<T,T> onSetWithOld) => runtimeSingle.onSetWithOld -= onSetWithOld;
        
        public void SubOnRemove(Action onRemove) => runtimeSingle.onRemove += onRemove;
        public void UnSubOnRemove(Action onRemove) => runtimeSingle.onRemove -= onRemove;
        
        public void SubOnRemoveWithOld(Action<T> onRemoveWithOld) => runtimeSingle.onRemoveWithOld += onRemoveWithOld;
        public void UnSubOnRemoveWithOld(Action<T> onRemoveWithOld) => runtimeSingle.onRemoveWithOld -= onRemoveWithOld;
    }
}