using System;
using NuiN.ScriptableHarmony.RuntimeSingle.Base;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NuiN.ScriptableHarmony.RuntimeSingle.References.Base
{
    [Serializable]
    public abstract class ReferenceRuntimeSingleBase<T>
    {
        [SerializeField] protected RuntimeSingleBaseSO<T> runtimeSingle;
        
        public bool IsNull => runtimeSingle == null;
        
        public void SetToResource(string resourceName) => runtimeSingle = Resources.Load<RuntimeSingleBaseSO<T>>(resourceName);
        
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