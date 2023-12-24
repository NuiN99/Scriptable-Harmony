using System;
using System.Collections.ObjectModel;
using NuiN.ScriptableHarmony.ListVariable.References.Base;

namespace NuiN.ScriptableHarmony.References
{
    [Serializable]
    public class GetListVariable<T> : ReferenceScriptableListVariableBase<T>
    {
        public ReadOnlyCollection<T> Items => list.values.AsReadOnly();
    }
}
