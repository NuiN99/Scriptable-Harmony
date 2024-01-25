using System;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    [Serializable]
    public abstract class ScriptableDictionaryReference<TKey,TValue>
    {
        [SerializeField] protected ScriptableDictionarySO<TKey,TValue> dictionary;
        
        public bool IsNull => dictionary == null;
        
        public void SetToResource(string resourceName) => dictionary = Resources.Load<ScriptableDictionarySO<TKey,TValue>>(resourceName);
    }
}
