using System;
using System.Collections.Generic;
using NuiN.ScriptableHarmony.Core;

namespace NuiN.ScriptableHarmony
{
    [Serializable]
    public class GetDictionary<T,TU> : ScriptableDictionaryReference<T,TU>
    {
        public Dictionary<T, TU> Dictionary => dictionary.dictionary;
    }
}
