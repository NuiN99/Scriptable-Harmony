using System;
using NuiN.ScriptableHarmony.RuntimeSingle.References.Base;
using Object = UnityEngine.Object;

namespace NuiN.ScriptableHarmony.References
{
    [Serializable]
    public class GetRuntimeSingle<T> : ReferenceRuntimeSingleBase<T>
    {
        public T Entity => runtimeSingle.entity;
    }
}