using NuiN.NExtensions;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    public abstract class SHBaseSO : ScriptableObject
    {
        protected abstract GetSetReferencesContainer GettersAndSetters { get; set; }
        public abstract bool LogActions { get; }

        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            EditorApplication.quitting += ClearDebugReferences;
            Selection.selectionChanged += OnSelectedInProjectWindow;
#endif
        }
        protected virtual void OnDisable()
        {
#if UNITY_EDITOR
            EditorApplication.quitting -= ClearDebugReferences;
            Selection.selectionChanged -= OnSelectedInProjectWindow;
#endif
        }
    
#if UNITY_EDITOR
        void Reset() => AssignDebugReferences();
        
        void OnSelectedInProjectWindow()
        {
            if(this) EditorUtility.SetDirty(this);
            GettersAndSetters?.Clear();
            
            if(Selection.activeObject == this) AssignDebugReferences();
        }

        void ClearDebugReferences()
        {
            if(this) EditorUtility.SetDirty(this);
            GettersAndSetters?.Clear();
        }
    
        void AssignDebugReferences()
        {
            GettersAndSetters?.FindObjectsAndAssignReferences(this);
        }
#endif
    }
}
