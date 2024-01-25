using System;
using System.Collections.Generic;
using NuiN.ScriptableHarmony.Editor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    public class RuntimeSetSO<T> : RuntimeObjectBaseSO<T>
    {
        [SerializeField] [TextArea] string description;
        
        [TypeMismatchFix] public List<T> entities = new();

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
        [SerializeField] RuntimeObjectReferencesContainer componentHolders = new("runtimeSet", typeof(RuntimeSetComponent<T>), typeof(SetRuntimeSet<T>));
        [SerializeField] GetSetReferencesContainer gettersAndSetters = new("runtimeSet", typeof(RuntimeSetReference<T>), typeof(GetRuntimeSet<T>), typeof(SetRuntimeSet<T>));
        protected override RuntimeObjectReferencesContainer ComponentHolders { get => componentHolders; set => componentHolders = value; }
        protected override GetSetReferencesContainer GettersAndSetters { get => gettersAndSetters; set => gettersAndSetters = value; }
        public override bool LogActions => logActions;
        
        protected override void ResetValue() => entities.Clear();
    }
}


