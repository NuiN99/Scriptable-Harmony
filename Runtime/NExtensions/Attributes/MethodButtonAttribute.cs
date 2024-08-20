using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NuiN.NExtensions
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
}