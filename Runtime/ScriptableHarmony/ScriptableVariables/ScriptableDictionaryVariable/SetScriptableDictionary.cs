using System;
using System.Collections.Generic;
using System.Diagnostics;
using NuiN.ScriptableHarmony.Core;
using UnityEditor;

namespace NuiN.ScriptableHarmony
{
    [Serializable]
    public class SetScriptableDictionary<TKey,TValue> : ScriptableDictionaryReference<TKey,TValue>
    {
        public IReadOnlyDictionary<TKey,TValue> Dictionary => dictionary.Dictionary;
        Dictionary<TKey, TValue> InternalDictionary => (Dictionary<TKey, TValue>)Dictionary;

        public bool TryAdd(TKey key, TValue value)
        {
            bool added = InternalDictionary.TryAdd(key, value);
            return added;
        }

        public void ResetDictionary()
        {
            dictionary.ResetDictionaryToDefault();
        }
    }
}
