using System;
using NuiN.ScriptableHarmony.Variable.References.Base;

namespace NuiN.ScriptableHarmony.References
{
    [Serializable]
    public class GetVariable<T> : ReferenceScriptableVariableBase<T>
    {
        public T Val => variable.value;
        public T DefaultVal => variable.DefaultValue;
    }
}
