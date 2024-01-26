using NuiN.ScriptableHarmony.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NuiN.ScriptableHarmony.Core
{
    public abstract class ScriptableVariableLifetimeSO<T> : SHBaseSO<T>
    {
        [SerializeField] [TextArea] string description;
    
        protected new virtual void OnEnable()
        {
            base.OnEnable();
            SORuntimeHelper.SubOnLoad(SaveDefaultValue);
            SceneManager.activeSceneChanged += ResetValueOnSceneLoad;
            ScriptableHarmonyManager.OnResetAllVariableObjects += ResetValueToDefault;
#if UNITY_EDITOR
            EditorApplication.quitting += ResetValueToDefault;
            EditorApplication.playModeStateChanged += ResetValueOnStoppedPlaying;
#endif
        }
        new void OnDisable()
        {
            base.OnDisable();
            SORuntimeHelper.UnSubOnLoad(SaveDefaultValue);
            SceneManager.activeSceneChanged -= ResetValueOnSceneLoad;
            ScriptableHarmonyManager.OnResetAllVariableObjects -= ResetValueToDefault;
#if  UNITY_EDITOR
            EditorApplication.quitting -= ResetValueToDefault;
            EditorApplication.playModeStateChanged -= ResetValueOnStoppedPlaying;
#endif
        }

        [MethodButton("Save Value", true)]
        protected abstract void SaveDefaultValue();
        
        [MethodButton("Reset to Default", true)]
        protected abstract void ResetValueToDefault();
        protected abstract bool ResetsOnSceneLoad();
    
        void ResetValueOnSceneLoad(Scene s1, Scene s2)
        {
            if (ResetsOnSceneLoad()) ResetValueToDefault();
        }
        
#if UNITY_EDITOR
        void ResetValueOnStoppedPlaying(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode) ResetValueToDefault();
        }
#endif
    }
}
