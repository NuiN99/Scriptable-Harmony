using System.Collections.Generic;
using System.Linq;
using NuiN.NExtensions;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    public class ScriptableDictionarySO<TKey,TValue> : ScriptableVariableLifetimeSO<TKey>
    {
        [SerializeField] SerializedDictionary<TKey, TValue> dictionary;
        [SerializeField, ReadOnlyPlayMode] SerializedDictionary<TKey, TValue> defaultDictionary;
        
        [SerializeField] RuntimeOptions runtimeOptions;

        [Header("Debugging")]
        [SerializeField] bool logActions;
        [SerializeField] GetSetReferencesContainer gettersAndSetters = new("list", typeof(ScriptableDictionaryReference<TKey,TValue>), typeof(GetScriptableDictionary<TKey,TValue>), typeof(SetScriptableDictionary<TKey,TValue>));
        protected override GetSetReferencesContainer GettersAndSetters { get => gettersAndSetters;set => gettersAndSetters = value; }

        public Dictionary<TKey, TValue> Dictionary => dictionary;
        public Dictionary<TKey,TValue> DefaultValues => defaultDictionary;
        public override bool LogActions => logActions;
        public override RuntimeOptions RuntimeOptions => runtimeOptions;
        
        protected override void SaveDefaultValue()
        {
            defaultDictionary = new SerializedDictionary<TKey, TValue>(dictionary);
        }

        protected override void ResetValueToDefault()
        {
            dictionary = new SerializedDictionary<TKey, TValue>(defaultDictionary);
        }
        
        public void ResetDictionaryToDefault()
        {
            ResetValueToDefault();
        }

        protected override void InvokeOnChangeEvent()
        {
            // todo - add events
        }
    }
}
