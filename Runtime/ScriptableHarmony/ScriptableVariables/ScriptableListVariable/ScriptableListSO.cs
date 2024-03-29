using System;
using System.Collections.Generic;
using NuiN.NExtensions;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    public class ScriptableListSO<T> : ScriptableVariableLifetimeSO<T>
    {
        public List<T> values = new();
        [SerializeField] [ReadOnlyPlayMode] List<T> defaultValues = new();
        
        [SerializeField] RuntimeOptions runtimeOptions;
        
        public Action<T> onAdd;
        public Action<List<T>,T> onAddWithOld;
        
        public Action<List<T>> onAddWithList;
        public Action<List<T>,List<T>> onAddWithListWithOld;

        public Action<T> onRemove;
        public Action<List<T>,T> onRemoveWithOld;
        
        public Action<List<T>> onRemoveWithList;
        public Action<List<T>,List<T>> onRemoveWithListWithOld;
        
        public Action<List<T>> onReplace;
        public Action<List<T>,List<T>> onReplaceWithOld;
        
        public Action onClear;
        public Action<List<T>> onClearWithOld;

        [Header("Debugging")] 
        [SerializeField] bool logActions;
        [SerializeField] GetSetReferencesContainer gettersAndSetters = new("list", typeof(ScriptableListReference<T>), typeof(GetScriptableList<T>), typeof(SetScriptableList<T>));
        protected override GetSetReferencesContainer GettersAndSetters { get => gettersAndSetters;set => gettersAndSetters = value; }
        public override RuntimeOptions RuntimeOptions => runtimeOptions;

        public List<T> DefaultValues => defaultValues;
        public override bool LogActions => logActions;


        void OnValidate()
        {
            if(!Application.isPlaying) defaultValues = new List<T>(values);
        }

        protected override void SaveDefaultValue() => defaultValues = new List<T>(values);
        protected override void ResetValueToDefault()
        {
            var oldValues = new List<T>(values);
            
            values = new List<T>(defaultValues);
            
            onReplaceWithOld?.Invoke(oldValues, values);
            onReplace?.Invoke(values);
        }

        protected override void InvokeOnChangeEvent()
        {
            onReplaceWithOld?.Invoke(values, values);
            onReplace?.Invoke(values);
        }
    }
}
