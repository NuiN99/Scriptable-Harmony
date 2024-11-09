#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NuiN.NExtensions
{
	[CustomEditor(typeof(Object), true), CanEditMultipleObjects]
    public class AttributesEditor : UnityEditor.Editor
    {
        void OnEnable() => RuntimeHelper.SubOnUpdate(Repaint);
        void OnDisable() => RuntimeHelper.UnSubOnUpdate(Repaint);

        public override void OnInspectorGUI()
        {
            MonoBehaviour monoTarget = target as MonoBehaviour;
            ScriptableObject soTarget = target as ScriptableObject;

            if (monoTarget == null && soTarget == null)
            {
                DrawDefaultInspector();
                return;
            }

            GUI.enabled = false;
            const string scriptLabel = "Script";
            if (target == monoTarget)
            {
                EditorGUILayout.ObjectField(scriptLabel, MonoScript.FromMonoBehaviour(monoTarget), typeof(MonoScript), false);
            }
            else if (target == soTarget)
            {
                EditorGUILayout.ObjectField(scriptLabel, MonoScript.FromScriptableObject(soTarget), typeof(MonoScript), false);
            }
            GUI.enabled = true;

            DrawMethodButtons();

            if (Application.isPlaying)
            {
                this.DrawShowInInspector();
            }

            serializedObject.Update();

            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);
            while (property.NextVisible(false))
            {
                EditorGUILayout.PropertyField(property, true);
            }

            serializedObject.ApplyModifiedProperties();
        }

        void DrawMethodButtons()
        {
            var processedMethods = new HashSet<string>();
            Type currentType = target.GetType();
            while (currentType != null && currentType != typeof(object))
            {
                MethodInfo[] methods = currentType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(method => method.GetCustomAttributes(typeof(MethodButtonAttribute), true).Length > 0)
                    .ToArray();

                foreach (var method in methods)
                {
                    string methodSignature = GetMethodSignature(method);

                    if (processedMethods.Contains(methodSignature))
                        continue;

                    object[] attributes = method.GetCustomAttributes(typeof(MethodButtonAttribute), true);
                    foreach (var attr in attributes)
                    {
                        MethodButtonAttribute attribute = (MethodButtonAttribute)attr;
                        string buttonLabel = attribute == null ? method.Name : attribute.label;

                        if (attribute == null || (attribute.onlyShowInPlayMode && !Application.isPlaying)) continue;

                        ParameterInfo[] methodParams = method.GetParameters();
                        object[] defaultParameters = new object[methodParams.Length];
                        for (int i = 0; i < methodParams.Length; i++)
                        {
                            Type paramType = methodParams[i].ParameterType;

                            if (!paramType.IsValueType) continue;
                            
                            if (methodParams[i].DefaultValue != null)
                            {
                                defaultParameters[i] = methodParams[i].DefaultValue;
                            }
                            else
                            {
                                defaultParameters[i] = Activator.CreateInstance(paramType);
                            }
                        }
                        
                        if (GUILayout.Button(buttonLabel))
                            method.Invoke(target, defaultParameters);

                        processedMethods.Add(methodSignature);
                    }
                }

                currentType = currentType.BaseType;
            }

            return;
            
            string GetMethodSignature(MethodInfo method)
            {
                ParameterInfo[] parameterInfos = method.GetParameters();
                var parameterSignatures = parameterInfos
                    .Select(p => p.ParameterType.FullName + " " + p.Name) // You can choose to omit the name if not needed
                    .ToArray();

                return method.Name + "(" + string.Join(", ", parameterSignatures) + ")";
            }
        }
    }
}

#endif