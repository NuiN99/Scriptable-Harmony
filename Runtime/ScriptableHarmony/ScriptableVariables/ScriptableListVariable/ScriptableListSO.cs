using System;
using System.Collections.Generic;
using NuiN.ScriptableHarmony.Base;
using NuiN.ScriptableHarmony.Editor.Attributes;
using NuiN.ScriptableHarmony.Internal.Helpers;
using NuiN.ScriptableHarmony.ListVariable.References.Base;
using NuiN.ScriptableHarmony.References;
using UnityEngine;

namespace NuiN.ScriptableHarmony.ListVariable.Base
{
    public class ScriptableListSO<T> : ScriptableVariableLifetimeSO<T>
    {
        public List<T> values = new();
        [SerializeField] [ReadOnlyPlayMode] List<T> defaultValues = new();
        
        [Header("Value Persistence")]
        [SerializeField] ResetOn resetOn;
        
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
        [SerializeField] bool logActions = true;
        [SerializeField] GetSetReferencesContainer gettersAndSetters = new("list", typeof(ScriptableListReference<T>), typeof(GetList<T>), typeof(SetListVariable<T>));
        protected override GetSetReferencesContainer GettersAndSetters { get => gettersAndSetters;set => gettersAndSetters = value; }
        
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

        protected override bool ResetsOnSceneLoad() => resetOn == ResetOn.SceneLoad;
    }
}
