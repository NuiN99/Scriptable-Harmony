using System;
using UnityEngine;

namespace NuiN.NExtensions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodButtonAttribute : PropertyAttribute
    {
        public readonly string label;
        public readonly bool onlyShowInPlayMode;
        public MethodButtonAttribute(string label, bool onlyShowInPlayMode = false, object[] parameters = null)
        {
            this.label = label;
            this.onlyShowInPlayMode = onlyShowInPlayMode;
        }
    }
}