#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NuiN.NExtensions.Editor
{
    internal class ComponentInjector : AssetModificationProcessor
    {
        // inject components when unity saves
        static string[] OnWillSaveAssets(string[] paths)
        {
            InjectComponentsInScene();
            InjectComponentsInPrefabs();
            return paths;
        }
        
        static void InjectComponentsInScene()
        {
            foreach (MonoBehaviour monoBehaviourInstance in Object.FindObjectsOfType<MonoBehaviour>())
            {
                InjectComponents(monoBehaviourInstance);
            }
        }
        
        static void InjectComponentsInPrefabs()
        {
            string[] allPrefabs = AssetDatabase.FindAssets("t:Prefab");

            foreach (string prefabGuid in allPrefabs)
            {
                string path = AssetDatabase.GUIDToAssetPath(prefabGuid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                MonoBehaviour[] monoBehaviours = prefab.GetComponentsInChildren<MonoBehaviour>(true);

                foreach (MonoBehaviour monoBehaviourInstance in monoBehaviours)
                {
                    InjectComponents(monoBehaviourInstance);
                }
            }
        }
        
        static void InjectComponents(Component monoBehaviourInstance)
        {
            Type type = monoBehaviourInstance.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            InjectMembers(fields, monoBehaviourInstance);
            InjectMembers(properties, monoBehaviourInstance);
        }

        static void InjectMembers(IEnumerable<MemberInfo> members, Component monoBehaviourInstance)
        {
            foreach (MemberInfo member in members)
            {
                InjectComponentAttribute attribute = member.GetCustomAttribute<InjectComponentAttribute>();
                if (attribute == null) continue;
                
                Type type = null;
                Component component = null;
                Action setAction = null;
                if (member is FieldInfo field)
                {
                    if (field.GetValue(monoBehaviourInstance) != null)
                    {
                        if (!field.GetValue(monoBehaviourInstance).Equals(null))
                        {
                            continue;
                        }
                    }
                    
                    type = field.FieldType;
                    component = GetComponent(monoBehaviourInstance, type, attribute);
                    setAction = () => field.SetValue(monoBehaviourInstance, component);
                }
                else if (member is PropertyInfo property)
                {
                    if (property.GetValue(monoBehaviourInstance) != null)
                    {
                        if (!property.GetValue(monoBehaviourInstance).Equals(null))
                        {
                            continue;
                        }
                    }
                    
                    type = property.PropertyType;
                    component = GetComponent(monoBehaviourInstance, type, attribute);
                    setAction = () => property.SetValue(monoBehaviourInstance, component);
                }
                
                if (component != null)
                {
                    setAction.Invoke();
                    EditorUtility.SetDirty(monoBehaviourInstance);
                }
                else
                {
                    WarnInjectionFailed(type, monoBehaviourInstance);
                }
            }
        }

        static Component GetComponent(Component monoBehaviourInstance, Type componentType, InjectComponentAttribute attribute)
        {
            Component component = monoBehaviourInstance.GetComponent(componentType);
            if (component != null) return component;

            switch (attribute.searchOrder)
            {
                case SearchOrder.ChildrenFirst:
                {
                    component = monoBehaviourInstance.GetComponentInChildren(componentType, true);
                    if(component == null) component = monoBehaviourInstance.GetComponentInParent(componentType, true);
                    break;
                }
                case SearchOrder.ParentsFirst:
                {
                    component = monoBehaviourInstance.GetComponentInParent(componentType, true);
                    if(component == null) component = monoBehaviourInstance.GetComponentInChildren(componentType, true);
                    break;
                }
            }

            if (component == null && attribute.injectOptions == InjectOptions.AddComponent)
            {
                component = monoBehaviourInstance.gameObject.AddComponent(componentType);
            }
            
            return component;
        }

        static void WarnInjectionFailed(Type componentType, Component instance)
        {
            Debug.LogWarning($"Component of type <color=yellow>{componentType.Name}</color> not found on <color=white>{instance.name}</color> while attempting injection", instance);
        }
    }
}
#endif