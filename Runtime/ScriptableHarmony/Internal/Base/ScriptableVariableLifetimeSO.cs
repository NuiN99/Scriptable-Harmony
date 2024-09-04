using NuiN.NExtensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NuiN.ScriptableHarmony.Core
{
    public abstract class ScriptableVariableLifetimeSO<T> : SHBaseSO
    {
        [SerializeField] [TextArea] string description;
        public abstract RuntimeOptions RuntimeOptions { get; }

        protected new virtual void OnEnable()
        {
            base.OnEnable();
            RuntimeHelper.SubOnLoad(SaveDefaultValue);
            SceneManager.activeSceneChanged += OnSceneLoad;
            ScriptableHarmonyManager.OnResetAllVariableObjects += ResetValueToDefault;
#if UNITY_EDITOR
            EditorApplication.quitting += ResetValueToDefault;
            EditorApplication.playModeStateChanged += OnStoppedPlaying;
#endif
        }
        new void OnDisable()
        {
            base.OnDisable();
            RuntimeHelper.UnSubOnLoad(SaveDefaultValue);
            SceneManager.activeSceneChanged -= OnSceneLoad;
            ScriptableHarmonyManager.OnResetAllVariableObjects -= ResetValueToDefault;
#if  UNITY_EDITOR
            EditorApplication.quitting -= ResetValueToDefault;
            EditorApplication.playModeStateChanged -= OnStoppedPlaying;
#endif
        }

        [MethodButton("Save Value", true)]
        protected abstract void SaveDefaultValue();
        
        [MethodButton("Reset to Default", true)]
        protected abstract void ResetValueToDefault();

        protected abstract void InvokeOnChangeEvent();
    
        void OnSceneLoad(Scene s1, Scene s2)
        {
            switch (RuntimeOptions)
            {
                case RuntimeOptions.ResetOnSceneLoad: ResetValueToDefault(); break;
                case RuntimeOptions.InvokeOnSceneLoad: InvokeOnChangeEvent(); break;
            }
        }
        
#if UNITY_EDITOR
        void OnStoppedPlaying(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode) ResetValueToDefault();
        }
#endif
    }
}
