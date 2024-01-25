using System;
using NuiN.ScriptableHarmony.Variable.Base;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Variable.References.Base
{
    [Serializable]
    public abstract class ScriptableVariableReference<T>
    {
        [SerializeField] protected ScriptableVariableSO<T> variable;

        public bool IsNull => variable == null;

        public void SetToResource(string resourceName) => variable = Resources.Load<ScriptableVariableSO<T>>(resourceName);
        
        public void SubOnChange(Action<T> onChange) => variable.onChange += onChange;
        public void UnSubOnChange(Action<T> onChange) => variable.onChange -= onChange;

        public void SubOnChangeWithOld(Action<T, T> onChangeWithOld) => variable.onChangeWithOld += onChangeWithOld;
        public void UnSubOnChangeWithOld(Action<T, T> onChangeWithOld) => variable.onChangeWithOld -= onChangeWithOld;
    }
}
