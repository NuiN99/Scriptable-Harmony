using System;
using NuiN.ScriptableHarmony.Core;

namespace NuiN.ScriptableHarmony
{
    [Serializable]
    public class GetScriptableVariable<T> : ScriptableVariableReference<T>
    {
        public T Val => variable.value;
        public T DefaultVal => variable.DefaultValue;
    }
}
