using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NuiN.ScriptableHarmony.Core;

namespace NuiN.ScriptableHarmony
{
    [Serializable]
    public class GetRuntimeSet<T> : RuntimeSetReference<T>, IEnumerable<T>
    {
        public ReadOnlyCollection<T> Entities => runtimeSet.entities.AsReadOnly();
        
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Entities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Entities.GetEnumerator();
        
        public bool Contains(T item) => Entities.Contains(item);
        public int Count => Entities.Count;
        public T this[int index] => Entities[index];
    }
}