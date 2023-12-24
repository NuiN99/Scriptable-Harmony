using NuiN.ScriptableHarmony.Events;
using NuiN.ScriptableHarmony.Internal.Helpers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NuiN.ScriptableHarmony.Base
{
    public abstract class RuntimeObjectBaseSO<T> : ScriptableObjectBaseSO<T>
    {
        protected abstract RuntimeObjectReferencesContainer ComponentHolders { get; set; }
    
        new void OnEnable()
        {
            SceneManager.sceneUnloaded += ResetOnSceneUnloaded;
            VariableEvents.OnResetAllVariableObjects += ResetValue;
#if UNITY_EDITOR
            base.OnEnable();
            EditorApplication.playModeStateChanged += ResetValueOnStoppedPlaying;
            Selection.selectionChanged += OnSelectedInProjectWindow;
#endif
        }
        new void OnDisable()
        {
            SceneManager.sceneUnloaded -= ResetOnSceneUnloaded;
            VariableEvents.OnResetAllVariableObjects -= ResetValue;
#if UNITY_EDITOR
            base.OnDisable();
            EditorApplication.playModeStateChanged -= ResetValueOnStoppedPlaying;
            Selection.selectionChanged -= OnSelectedInProjectWindow;
#endif
        }
    
        protected abstract void ResetValue();
        
        void ResetOnSceneUnloaded(Scene scene) => ResetValue();
    
#if UNITY_EDITOR
        void ResetValueOnStoppedPlaying(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode) ResetValue();
        }
        
        void OnSelectedInProjectWindow()
        {
            ComponentHolders?.Clear();
            if (Selection.activeObject != this) return;
            AssignComponentDebugReferences();
        }
    
        void AssignComponentDebugReferences()
        {
            GameObject[] sceneObjs = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            ComponentHolders.FindObjectsAndAssignReferences(this, sceneObjs);
        }
#endif
    }
}

