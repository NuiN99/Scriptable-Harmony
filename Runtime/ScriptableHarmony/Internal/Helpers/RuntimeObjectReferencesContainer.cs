using System;
using System.Collections.Generic;
using System.Reflection;
using NuiN.ScriptableHarmony.Editor.Attributes;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Internal.Helpers
{
    [Serializable]
    public class RuntimeObjectReferencesContainer : ReferencesContainerBase
    {
        [TypeMismatchFix]
        public List<Component> prefabs;
        [TypeMismatchFix]
        public List<Component> scene;
        
        Type _setterType;

        public RuntimeObjectReferencesContainer(string fieldName, Type baseType, Type setterType) : base(fieldName, baseType)
        {
#if UNITY_EDITOR
            _setterType = setterType;
#endif
        }
        
        public override int TotalReferencesCount()
        {
#if UNITY_EDITOR
            return prefabs.Count + scene.Count;
#endif
#if !UNITY_EDITOR
            return 0;
#endif
        }

        public override bool ListsAreNull()
        {
#if UNITY_EDITOR
            return prefabs == null || scene == null;
#endif
#if !UNITY_EDITOR
            return true;
#endif
        }

        public override void Clear()
        {
#if UNITY_EDITOR
            prefabs?.Clear();
            scene?.Clear();
#endif
        }
        
        protected override void CheckComponentAndAssign(object variableCaller, Component component, ObjectsToSearch objectsToSearch)
        {
#if UNITY_EDITOR
            Type componentType = component.GetType();
            
            Type componentBaseType = componentType.BaseType;
            if (componentBaseType == null || componentBaseType != baseType) return;
            
            FieldInfo[] fields =
                baseType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                Type type = field.FieldType;
                if (!type.IsGenericType) continue;

                object variableField = field.GetValue(component);
                if (variableField == null || !_setterType.IsInstanceOfType(variableField)) continue;

                FieldInfo variableFieldInfo =
                    _setterType.GetField(fieldName,
                        BindingFlags.Instance | BindingFlags.NonPublic);

                if (variableFieldInfo == null ||
                    !ReferenceEquals(variableFieldInfo.GetValue(variableField), variableCaller)) continue;

                switch (objectsToSearch)
                {
                    case ObjectsToSearch.Prefabs: prefabs?.Add(component); break;
                    case ObjectsToSearch.Scene: scene?.Add(component); break;
                }
            }
#endif
        }
    }
}
