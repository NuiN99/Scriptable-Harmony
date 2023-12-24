using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NuiN.ScriptableHarmony.Editor.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodButtonAttribute : PropertyAttribute
    {
        public readonly string label;
        public readonly bool onlyShowInPlayMode;
        public readonly object[] parameters;
        public MethodButtonAttribute(string label, bool onlyShowInPlayMode = false, object[] parameters = null)
        {
            this.label = label;
            this.parameters = parameters;
            this.onlyShowInPlayMode = onlyShowInPlayMode;
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(Object), true)]
    public class MethodButtonAttributeDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Object script = (Object)target;
            MethodInfo[] methods = script.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(method => method.GetCustomAttributes(typeof(MethodButtonAttribute), true).Length > 0)
                .ToArray();

            foreach (var method in methods)
            {
                MethodButtonAttribute attribute = (MethodButtonAttribute)method.GetCustomAttributes(typeof(MethodButtonAttribute), true)[0];
                string buttonLabel = attribute == null ? method.Name : attribute.label;

                if (attribute == null || (attribute.onlyShowInPlayMode && !Application.isPlaying)) continue;
                if (GUILayout.Button(buttonLabel)) method.Invoke(script, attribute?.parameters);
            }
            
            base.OnInspectorGUI();
        }
    }
#endif
}