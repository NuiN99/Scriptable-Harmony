using System;
using System.Threading.Tasks;
using NuiN.ScriptableHarmony.Base;
using NuiN.ScriptableHarmony.Editor.Attributes;
using NuiN.ScriptableHarmony.Internal.Helpers;
using NuiN.ScriptableHarmony.References;
using NuiN.ScriptableHarmony.Variable.References.Base;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Variable.Base
{
    public class ScriptableVariableBaseSO<T> : ScriptableVariableLifetimeBaseSO<T>
    {
        T _prevValue;
        
        public T value;
        [SerializeField] [ReadOnlyPlayMode] T defaultValue;
        
        [Header("Value Persistence")]
        [SerializeField] ResetOn resetOn;
        
        public Action<T> onChange;
        public Action<T, T> onChangeWithOld;

        [Header("Debugging")] 
        [SerializeField] bool logActions = true;
        [SerializeField] GetSetReferencesContainer gettersAndSetters = new("variable", typeof(ReferenceScriptableVariableBase<T>), typeof(GetVariable<T>), typeof(SetVariable<T>));
        
        protected override GetSetReferencesContainer GettersAndSetters { get => gettersAndSetters; set => gettersAndSetters = value; }
        public T DefaultValue => defaultValue;
        public override bool LogActions => logActions;
        
        void OnValidate()
        {
            InvokeOnValueChangedInEditor();
            if(!Application.isPlaying) defaultValue = value;
        }
        async void InvokeOnValueChangedInEditor()
        {
            if (!Application.isPlaying || Equals(value, _prevValue)) return;
            
            // yield until next frame to avoid warnings when using sliders
            await Task.Yield();
            
            onChangeWithOld?.Invoke(_prevValue, value);
            onChange?.Invoke(value);
            _prevValue = value;
        }
        
        protected override void SaveDefaultValue() => defaultValue = value;
        protected override void ResetValueToDefault()
        {
            T oldValue = value;
            value = defaultValue;
        }

        protected override bool ResetsOnSceneLoad() => resetOn == ResetOn.SceneLoad;
    }
}

