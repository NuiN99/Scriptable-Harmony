using System;
using System.Collections.Generic;
using NuiN.ScriptableHarmony.ListVariable.References.Base;

namespace NuiN.ScriptableHarmony.References
{
    [Serializable]
    public class GetDictionaryVariable<T,TU> : ReferenceScriptableDictionaryVariableBase<T,TU>
    {
        public Dictionary<T, TU> Dictionary => dictionary.dictionary;
    }
}
