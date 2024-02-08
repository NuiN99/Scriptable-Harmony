using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NuiN.ScriptableHarmony.Core
{
    public abstract class RuntimeObjectBaseSO<T> : SHBaseSO
    {
        protected abstract RuntimeObjectReferencesContainer ComponentHolders { get; set; }
    
        new void OnEnable()
        {
            ScriptableHarmonyManager.OnResetAllVariableObjects += ResetValue;
#if UNITY_EDITOR
            base.OnEnable();
            Selection.selectionChanged += OnSelectedInProjectWindow;
#endif
        }
        new void OnDisable()
        {
            ScriptableHarmonyManager.OnResetAllVariableObjects -= ResetValue;
#if UNITY_EDITOR
            base.OnDisable();
            Selection.selectionChanged -= OnSelectedInProjectWindow;
#endif
        }
    
        protected abstract void ResetValue();
        
#if UNITY_EDITOR
        
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

