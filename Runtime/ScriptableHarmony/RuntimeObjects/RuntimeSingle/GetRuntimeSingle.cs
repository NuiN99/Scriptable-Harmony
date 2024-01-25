using System;
using NuiN.ScriptableHarmony.RuntimeSingle.References.Base;

namespace NuiN.ScriptableHarmony.References
{
    [Serializable]
    public class GetRuntimeSingle<T> : ReferenceRuntimeSingleBase<T>
    {
        public T Entity => runtimeSingle.entity;
    }
}