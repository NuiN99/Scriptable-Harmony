using System;
using NuiN.ScriptableHarmony.RuntimeSingle.References.Base;

namespace NuiN.ScriptableHarmony.References
{
    [Serializable]
    public class GetRuntimeSingle<T> : RuntimeSingleReference<T>
    {
        public T Entity => runtimeSingle.entity;
    }
}