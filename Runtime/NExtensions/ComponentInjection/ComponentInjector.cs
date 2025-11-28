#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NuiN.NExtensions.Editor
{
    internal static class ComponentInjector
    {
        [MenuItem("Tools/SH/Inject Components")]
        static void InjectMenuItem()
        {
            InjectComponentsInScene();
            InjectComponentsInPrefabs();
        }
        
        static void InjectComponentsInScene()
        {
            foreach (MonoBehaviour monoBehaviourInstance in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
            {
                InjectComponents(monoBehaviourInstance);
            }
        }
        
        static void InjectComponentsInPrefabs()
        {
            string[] allPrefabs = AssetDatabase.FindAssets("t:Prefab", new[] {"Assets"});

            foreach (string prefabGuid in allPrefabs)
            {
                string path = AssetDatabase.GUIDToAssetPath(prefabGuid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                
                if(prefab == null) continue;

                MonoBehaviour[] monoBehaviours = prefab.GetComponentsInChildren<MonoBehaviour>(true);
                
                foreach (MonoBehaviour monoBehaviourInstance in monoBehaviours)
                {
                    InjectComponents(monoBehaviourInstance);
                }
            }
        }
        
        static void InjectComponents(Component monoBehaviourInstance)
        {
            if (monoBehaviourInstance == null) return;
            
            Type type = monoBehaviourInstance.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            InjectFields(fields, monoBehaviourInstance);
        }

        static void InjectFields(IEnumerable<FieldInfo> fields, Component monoBehaviourInstance)
        {
            foreach (FieldInfo field in fields)
            {
                InjectComponentAttribute attribute = field.GetCustomAttribute<InjectComponentAttribute>();
                if (attribute == null) continue;
                
                if (field.GetValue(monoBehaviourInstance) != null)
                {
                    if (!field.GetValue(monoBehaviourInstance).Equals(null))
                    {
                        continue;
                    }
                }
                    
                Type type = field.FieldType;
                Component component = GetComponent(monoBehaviourInstance, type, attribute);
                if (component != null)
                {
                    field.SetValue(monoBehaviourInstance, component);
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
                if (!componentType.IsAssignableFrom(typeof(MonoBehaviour)))
                {
                    Debug.LogError($"Attempted to add non-MonoBehaviour component of type <color=red>{componentType.Name}</color> on <color=white>{GetMonobehaviourName(monoBehaviourInstance)}</color> while attempting injection", monoBehaviourInstance);
                    return component;
                }
                
                component = monoBehaviourInstance.gameObject.AddComponent(componentType);
            }
            
            return component;
        }

        static string GetMonobehaviourName(Component instance) 
            => $"{instance.name} -> {instance.GetType().Name}";

        static void WarnInjectionFailed(Type componentType, Component instance) 
            => Debug.LogWarning($"Component of type <color=yellow>{componentType.Name}</color> not found on <color=white>{GetMonobehaviourName(instance)}</color> while attempting injection", instance);
    }
}
#endif