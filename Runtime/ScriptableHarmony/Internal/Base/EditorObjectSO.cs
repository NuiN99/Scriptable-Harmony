using NuiN.ScriptableHarmony.Internal.Helpers;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Base
{
    public abstract class EditorObjectSO<T> : ScriptableObject
    {
        protected abstract GetSetReferencesContainer GettersAndSetters { get; set; }
        public abstract bool LogActions { get; }

        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            Selection.selectionChanged += OnSelectedInProjectWindow;
#endif
        }
        protected virtual void OnDisable()
        {
#if UNITY_EDITOR
            Selection.selectionChanged -= OnSelectedInProjectWindow;
#endif
        }
    
#if UNITY_EDITOR
        void Reset() => AssignDebugReferences();
        
        void OnSelectedInProjectWindow()
        {
            GettersAndSetters.Clear();
            if(this) EditorUtility.SetDirty(this);
            if (Selection.activeObject != this) return;
            AssignDebugReferences();
        }
    
        void AssignDebugReferences()
        {
            GameObject[] sceneObjs = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            GettersAndSetters.FindObjectsAndAssignReferences(this, sceneObjs);
        }
#endif
    }
}
