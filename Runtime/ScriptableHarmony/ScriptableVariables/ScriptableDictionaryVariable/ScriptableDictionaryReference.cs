using System;
using NuiN.ScriptableHarmony.ListVariable.Base;
using UnityEngine;

namespace NuiN.ScriptableHarmony.ListVariable.References.Base
{
    [Serializable]
    public abstract class ScriptableDictionaryReference<TKey,TValue>
    {
        [SerializeField] protected ScriptableDictionarySO<TKey,TValue> dictionary;
        
        public bool IsNull => dictionary == null;
        
        public void SetToResource(string resourceName) => dictionary = Resources.Load<ScriptableDictionarySO<TKey,TValue>>(resourceName);
    }
}
