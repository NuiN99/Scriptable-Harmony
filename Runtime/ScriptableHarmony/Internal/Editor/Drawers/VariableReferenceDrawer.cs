using NuiN.ScriptableHarmony.Core;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Editor
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(GetScriptableVariable<>))]
    internal class GetVariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.DrawGUI(SOType.Variable, Access.Getter, "variable", position, property, label, fieldInfo);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.GetPropertyHeight(property, label);
    }

    [CustomPropertyDrawer(typeof(SetScriptableVariable<>))]
    internal class SetVariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => SOReferenceGUIHelper.DrawGUI(SOType.Variable, Access.Setter, "variable", position, property, label, fieldInfo);
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.GetPropertyHeight(property, label);
    }
#endif
}

