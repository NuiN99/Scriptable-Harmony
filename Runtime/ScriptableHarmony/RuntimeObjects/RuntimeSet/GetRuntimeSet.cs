using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NuiN.ScriptableHarmony.RuntimeSet.References.Base;

namespace NuiN.ScriptableHarmony.References
{
    [Serializable]
    public class GetRuntimeSet<T> : RuntimeSetReference<T>, IEnumerable<T>
    {
        public ReadOnlyCollection<T> Entities => runtimeSet.entities.AsReadOnly();
        
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Entities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Entities.GetEnumerator();
    }
}