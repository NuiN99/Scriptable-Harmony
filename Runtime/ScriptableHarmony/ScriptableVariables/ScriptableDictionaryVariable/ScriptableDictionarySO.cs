using System.Collections.Generic;
using NuiN.NExtensions;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    public class ScriptableDictionarySO<TKey,TValue> : ScriptableVariableLifetimeSO<TKey>
    {
        public Dictionary<TKey,TValue> dictionary = new();
        public SerializableDictionary<TKey, TValue> serializedDictionary;

        Dictionary<TKey,TValue> _defaultDictionary = new();
        [SerializeField, ReadOnlyPlayMode] SerializableDictionary<TKey, TValue> serializedDefaultDictionary;
        
        [SerializeField] RuntimeOptions runtimeOptions;

        [Header("Debugging")] 
        [SerializeField] bool logActions;
        [SerializeField] GetSetReferencesContainer gettersAndSetters = new("list", typeof(ScriptableDictionaryReference<TKey,TValue>), typeof(GetScriptableDictionary<TKey,TValue>), typeof(SetScriptableDictionary<TKey,TValue>));
        protected override GetSetReferencesContainer GettersAndSetters { get => gettersAndSetters;set => gettersAndSetters = value; }
        
        public Dictionary<TKey,TValue> DefaultValues => _defaultDictionary;
        public override bool LogActions => logActions;
        public override RuntimeOptions RuntimeOptions => runtimeOptions;
        
        void OnValidate()
        {
            if(!Application.isPlaying) SaveDefaultValue();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            serializedDictionary ??= new SerializableDictionary<TKey, TValue>(ref dictionary);
            serializedDictionary.ValidateAndApply(ref dictionary);
        }

#if UNITY_EDITOR
        [MethodButton("ValidateDictionary")]
        public void ValidateDictionary()
        {
            Undo.RecordObject(this, "Validate and Apply");
            serializedDictionary.ValidateAndApply(ref dictionary);
            EditorUtility.SetDirty(this);
        }
#endif

        protected override void SaveDefaultValue()
        {
            _defaultDictionary = new Dictionary<TKey, TValue>(serializedDictionary.GetDictionary());
            serializedDictionary.ValidateAndApply(ref _defaultDictionary);

            serializedDefaultDictionary ??= new SerializableDictionary<TKey, TValue>(ref dictionary);
            serializedDefaultDictionary.SetValues(ref _defaultDictionary);
        }

        protected override void ResetValueToDefault()
        {
            dictionary = new Dictionary<TKey, TValue>(_defaultDictionary);
            serializedDictionary.Serialize(ref dictionary);
        }

        protected override void InvokeOnChangeEvent()
        {
            // todo - add events
        }
    }
}
