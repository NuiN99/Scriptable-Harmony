using System;
using NuiN.ScriptableHarmony.Base;
using NuiN.ScriptableHarmony.Editor.Attributes;
using NuiN.ScriptableHarmony.Internal.Helpers;
using NuiN.ScriptableHarmony.References;
using NuiN.ScriptableHarmony.RuntimeSingle.Components.Base;
using NuiN.ScriptableHarmony.RuntimeSingle.References.Base;
using UnityEngine;

namespace NuiN.ScriptableHarmony.RuntimeSingle.Base
{
    public class RuntimeSingleBaseSO<T> : RuntimeObjectBaseSO<T>
    {
        [SerializeField] [TextArea] string description;

        [TypeMismatchFix] public T entity;
        
        public Action<T> onSet;
        public Action<T, T> onSetWithOld;
        
        public Action onRemove;
        public Action<T> onRemoveWithOld;

        [Header("Debugging")]
        [SerializeField] bool logActions = true;
        [SerializeField] RuntimeObjectReferencesContainer componentHolders = new("runtimeSingle", typeof(RuntimeSingleItemComponentBase<T>), typeof(SetRuntimeSingle<T>));
        [SerializeField] GetSetReferencesContainer gettersAndSetters = new("runtimeSingle", typeof(ReferenceRuntimeSingleBase<T>), typeof(GetRuntimeSingle<T>), typeof(SetRuntimeSingle<T>));
        protected override RuntimeObjectReferencesContainer ComponentHolders { get => componentHolders; set => componentHolders = value; }
        protected override GetSetReferencesContainer GettersAndSetters { get => gettersAndSetters; set => gettersAndSetters = value; }
        public override bool LogActions => logActions;
        
        protected override void ResetValue() => entity = default;
    }
}


