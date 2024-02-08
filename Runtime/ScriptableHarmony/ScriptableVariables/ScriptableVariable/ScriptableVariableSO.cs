using System;
using System.Threading.Tasks;
using NuiN.NExtensions;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    public class ScriptableVariableSO<T> : ScriptableVariableLifetimeSO<T>
    {
        T _prevValue;
        
        public T value;
        [SerializeField] [ReadOnlyPlayMode] T defaultValue;
        
        [SerializeField] RuntimeOptions runtimeOptions;
        
        public Action<T> onChange;
        public Action<T, T> onChangeWithOld;

        [Header("Debugging")] 
        [SerializeField] bool logActions;
        [SerializeField] GetSetReferencesContainer gettersAndSetters = new("variable", typeof(ScriptableVariableReference<T>), typeof(GetScriptableVariable<T>), typeof(SetScriptableVariable<T>));

        protected override GetSetReferencesContainer GettersAndSetters { get => gettersAndSetters; set => gettersAndSetters = value; }
        public T DefaultValue => defaultValue;
        public override bool LogActions => logActions;
        public override RuntimeOptions RuntimeOptions => runtimeOptions;
        
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
            
            onChangeWithOld?.Invoke(oldValue, value);
            onChange?.Invoke(value);
        }

        protected override void InvokeOnChangeEvent()
        {
            onChangeWithOld?.Invoke(value, value);
            onChange?.Invoke(value);
        }
    }
}

