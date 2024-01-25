using System;
using NuiN.ScriptableHarmony.Editor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    public class RuntimeSingleSO<T> : RuntimeObjectBaseSO<T>
    {
        [SerializeField] [TextArea] string description;

        [TypeMismatchFix] public T entity;
        
        public Action<T> onSet;
        public Action<T, T> onSetWithOld;
        
        public Action onRemove;
        public Action<T> onRemoveWithOld;

        [Header("Debugging")]
        [SerializeField] bool logActions = true;
        [SerializeField] RuntimeObjectReferencesContainer componentHolders = new("runtimeSingle", typeof(RuntimeSingleComponent<T>), typeof(SetRuntimeSingle<T>));
        [SerializeField] GetSetReferencesContainer gettersAndSetters = new("runtimeSingle", typeof(RuntimeSingleReference<T>), typeof(GetRuntimeSingle<T>), typeof(SetRuntimeSingle<T>));
        protected override RuntimeObjectReferencesContainer ComponentHolders { get => componentHolders; set => componentHolders = value; }
        protected override GetSetReferencesContainer GettersAndSetters { get => gettersAndSetters; set => gettersAndSetters = value; }
        public override bool LogActions => logActions;
        
        protected override void ResetValue() => entity = default;
    }
}


