using System;
using System.Collections.Generic;
using NuiN.ScriptableHarmony.ListVariable.Base;
using UnityEngine;

namespace NuiN.ScriptableHarmony.ListVariable.References.Base
{
    [Serializable]
    public abstract class ReferenceScriptableDictionaryVariableBase<T,TU>
    {
        [SerializeField] protected ScriptableDictionaryVariableBaseSO<T,TU> dictionary;
    }
}
