using NuiN.NExtensions;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace NuiN.ScriptableHarmony.Core
{
    public abstract class RuntimeObjectSO : SHBaseSO
    {
        protected abstract RuntimeObjectReferencesContainer ComponentHolders { get; set; }
    
        new void OnEnable()
        {
            SceneManager.sceneUnloaded += ResetOnSceneUnloaded;
            ScriptableHarmonyManager.OnResetAllVariableObjects += ResetValue;
#if UNITY_EDITOR
            base.OnEnable();
            EditorApplication.playModeStateChanged += ResetValueOnStoppedPlaying;
            Selection.selectionChanged += OnSelectedInProjectWindow;
#endif
        }
        new void OnDisable()
        {
            SceneManager.sceneUnloaded -= ResetOnSceneUnloaded;
            ScriptableHarmonyManager.OnResetAllVariableObjects -= ResetValue;
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
            if(this) EditorUtility.SetDirty(this);
            ComponentHolders?.Clear();
        }
        
        [MethodButton("Refresh Debug References")]
        void AssignComponentDebugReferences()
        {
            ComponentHolders?.FindObjectsAndAssignReferences(this);
        }
#endif
    }
}

