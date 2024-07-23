#if UNITY_EDITOR
using System;
using System.Reflection;
using NuiN.CommandConsole;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NuiN.ScriptableHarmony.Editor
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

            foreach (FieldInfo field in fields)
            {
                if (field.GetValue() != null || field.GetCustomAttribute<InjectComponentAttribute>() == null) continue;
                
                Type componentType = field.FieldType;
                Component component = monoBehaviourInstance.GetComponent(componentType);
                if (component == null) component = monoBehaviourInstance.GetComponentInParent(componentType);
                if (component == null) component = monoBehaviourInstance.GetComponentInChildren(componentType);

                if (component != null)
                {
                    field.SetValue(monoBehaviourInstance, component);
                    EditorUtility.SetDirty(monoBehaviourInstance);
                }
                else
                {
                    Debug.LogWarning($"Component of type {componentType.Name} not found on {monoBehaviourInstance.name} when attempting injection", monoBehaviourInstance);
                }
            }

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                if (property.GetValue() != null ||  property.GetCustomAttribute<InjectComponentAttribute>() == null) continue;
                
                Type componentType = property.PropertyType;
                Component component = monoBehaviourInstance.GetComponent(componentType);
                if (component == null) component = monoBehaviourInstance.GetComponentInParent(componentType);
                if (component == null) component = monoBehaviourInstance.GetComponentInChildren(componentType);

                if (component != null)
                {
                    property.SetValue(monoBehaviourInstance, component);
                    EditorUtility.SetDirty(monoBehaviourInstance);
                }
                else
                {
                    Debug.LogWarning($"Component of type {componentType.Name} not found on {monoBehaviourInstance.name} when attempting injection", monoBehaviourInstance);
                }
            }
        }
    }
}
#endif