using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using NuiN.ScriptableHarmony.Internal.Helpers;
using NuiN.ScriptableHarmony.Internal.Logging;
using NuiN.ScriptableHarmony.ListVariable.Base;
using NuiN.ScriptableHarmony.ListVariable.References.Base;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace NuiN.ScriptableHarmony.References
{
    [Serializable]
    public class SetDictionaryVariable<TKey,TValue> : ReferenceScriptableDictionaryVariableBase<TKey,TValue>
    {
        public IReadOnlyDictionary<TKey,TValue> Dictionary => dictionary.dictionary;
        Dictionary<TKey, TValue> InternalDictionary => (Dictionary<TKey, TValue>)Dictionary;

        public bool TryAdd(TKey key, TValue value)
        {
            bool added = InternalDictionary.TryAdd(key, value);
            dictionary.serializedDictionary.Serialize(ref dictionary.dictionary);
            return added;
        }

        public void ResetDictionary()
        {
            dictionary.dictionary = new Dictionary<TKey, TValue>(dictionary.DefaultValues);
            dictionary.serializedDictionary.Serialize(ref dictionary.dictionary);
        }

        [Conditional("UNITY_EDITOR")]
        void SetDirty()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(dictionary);
#endif
        }
    }
}
