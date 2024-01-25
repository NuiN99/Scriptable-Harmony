using System;
using NuiN.ScriptableHarmony.Core;

namespace NuiN.ScriptableHarmony
{
    [Serializable]
    public class GetRuntimeSingle<T> : RuntimeSingleReference<T>
    {
        public T Entity => runtimeSingle.entity;
    }
}