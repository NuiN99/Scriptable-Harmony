using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    [Serializable]
    public abstract class ReferencesContainerBase
    {
        public enum ObjectsToSearch { Scene, Prefabs }
        
        protected Type baseType;
        protected string fieldName;

        public abstract void Clear();
        public abstract int TotalReferencesCount();
        public abstract bool ListsAreNull();
        protected abstract void CheckComponentAndAssign(object variableCaller, Component component, ObjectsToSearch objectsToSearch);

        protected ReferencesContainerBase(string fieldName, Type baseType)
        {
#if UNITY_EDITOR
            this.fieldName = fieldName;
            this.baseType = baseType;
#endif
        }
#if UNITY_EDITOR
        public void FindObjectsAndAssignReferences(object variableCaller, IEnumerable<GameObject> sceneObjs)
        {
            Clear();
                
            string[] guids = AssetDatabase.FindAssets( "t:Prefab" );
            GameObject[] allPrefabs = guids.Select(guid =>
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                return AssetDatabase.LoadAssetAtPath<GameObject>(path);
            }).ToArray();

            AssignReferences(variableCaller, allPrefabs, ObjectsToSearch.Prefabs);
            AssignReferences(variableCaller, sceneObjs, ObjectsToSearch.Scene);
        }
        
        void AssignReferences(object variableCaller, IEnumerable<GameObject> foundObjects, ObjectsToSearch objectsToSearch)
        {
            foreach (var obj in foundObjects)
            {
                Component[] components =
                    objectsToSearch switch
                    {
                        ObjectsToSearch.Prefabs => obj.GetComponentsInChildren<Component>(),
                        ObjectsToSearch.Scene => obj.GetComponents<Component>(),
                        _ => Array.Empty<Component>()
                    };

                foreach (var component in components)
                {
                    if (!component) continue;
                    CheckComponentAndAssign(variableCaller, component, objectsToSearch);
                }
            }
        }
#endif
    }
}