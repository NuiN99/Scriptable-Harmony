using System;
using System.Collections.Generic;
using System.Reflection;
using NuiN.ScriptableHarmony.Editor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
    [Serializable]
    public class GetSetReferencesContainer : ReferencesContainerBase
    {
        Type _getterType;
        Type _setterType;
        
        [Header("Prefabs")]
        [TypeMismatchFix]
        public List<Component> Getters;
        [TypeMismatchFix]
        public List<Component> Setters;
            
        [Header("Scene")]
        [TypeMismatchFix]
        public List<Component> getters;
        [TypeMismatchFix]
        public List<Component> setters;
        
        public GetSetReferencesContainer(string fieldName, Type baseType, Type getterType, Type setterType) : base(fieldName, baseType)
        {
#if UNITY_EDITOR
            _setterType = setterType;
            _getterType = getterType;
#endif
        }
        
        public override int TotalReferencesCount()
        {
#if UNITY_EDITOR
            return Setters.Count + Getters.Count + setters.Count + getters.Count;
#endif
#if !UNITY_EDITOR
            return 0;
#endif
        }

        public override bool ListsAreNull()
        {
#if UNITY_EDITOR
            return Setters == null || Getters == null || setters == null || getters == null;
#endif
#if !UNITY_EDITOR
            return true;
#endif
        }

        public override void Clear()
        {
#if UNITY_EDITOR
            Getters?.Clear();
            Setters?.Clear();
            getters?.Clear();
            setters?.Clear();
#endif
        }
        
        protected override void CheckComponentAndAssign(object variableCaller, Component component, ObjectsToSearch objectsToSearch)
        {
#if UNITY_EDITOR
            Type componentType = component.GetType();
            FieldInfo[] fields =
                componentType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                Type type = field.FieldType;
                if (!type.IsGenericType) continue;

                bool isGetter = type == _getterType;
                bool isSetter = type == _setterType;

                if (!isGetter && !isSetter) continue;
                        
                object variableField = field.GetValue(component);
                if (variableField == null || (!_getterType.IsInstanceOfType(variableField) && !_setterType.IsInstanceOfType(variableField))) return;

                FieldInfo variableFieldInfo =
                    baseType.GetField(fieldName,
                        BindingFlags.Instance | BindingFlags.NonPublic);

                if (variableFieldInfo == null ||
                    !ReferenceEquals(variableFieldInfo.GetValue(variableField), variableCaller)) continue;

                switch (objectsToSearch)
                {
                    case ObjectsToSearch.Prefabs when isGetter:Getters?.Add(component); break;
                    case ObjectsToSearch.Prefabs: Setters?.Add(component); break;
                    case ObjectsToSearch.Scene when isGetter: getters?.Add(component); break;
                    case ObjectsToSearch.Scene: setters?.Add(component); break;
                }
            }
#endif
        }
    }
}
