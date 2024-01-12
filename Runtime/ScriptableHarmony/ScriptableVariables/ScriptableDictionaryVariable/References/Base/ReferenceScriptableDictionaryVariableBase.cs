using System;
using System.Collections.Generic;
using NuiN.ScriptableHarmony.ListVariable.Base;
using UnityEngine;

namespace NuiN.ScriptableHarmony.ListVariable.References.Base
{
    [Serializable]
    public abstract class ReferenceScriptableDictionaryVariableBase<TKey,TValue>
    {
        [SerializeField] protected ScriptableDictionaryVariableBaseSO<TKey,TValue> dictionary;
        
        public bool IsNull => dictionary == null;
        
        public void SetToResource(string resourceName) => dictionary = Resources.Load<ScriptableDictionaryVariableBaseSO<TKey,TValue>>(resourceName);
    }
}
