using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NuiN.ScriptableHarmony.Core;

namespace NuiN.ScriptableHarmony
{
    [Serializable]
    public class GetList<T> : ScriptableListReference<T>, IEnumerable<T>
    {
        public ReadOnlyCollection<T> Items => list.values.AsReadOnly();
        
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
    }
}
