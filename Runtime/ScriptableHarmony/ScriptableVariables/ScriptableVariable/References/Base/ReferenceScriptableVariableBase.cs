using System;
using NuiN.ScriptableHarmony.Variable.Base;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Variable.References.Base
{
    [Serializable]
    public abstract class ReferenceScriptableVariableBase<T>
    {
        [SerializeField] protected ScriptableVariableBaseSO<T> variable;

        public void SetToResource(string resourceName) => variable = Resources.Load<ScriptableVariableBaseSO<T>>(resourceName);
        
        public void SubOnChange(Action<T> onChange) => variable.onChange += onChange;
        public void UnSubOnChange(Action<T> onChange) => variable.onChange -= onChange;

        public void SubOnChangeWithOld(Action<T, T> onChangeWithOld) => variable.onChangeWithOld += onChangeWithOld;
        public void UnSubOnChangeWithOld(Action<T, T> onChangeWithOld) => variable.onChangeWithOld -= onChangeWithOld;
    }
}
