using NuiN.ScriptableHarmony.Core.Editor.Helpers;
using NuiN.ScriptableHarmony.Internal.Helpers;
using NuiN.ScriptableHarmony.References;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Internal.Editor
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(GetRuntimeSingle<>))]
    internal class GetRuntimeSingleDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.VarRefGUI(SOType.RuntimeSingle, Access.Getter, "runtimeSingle", position, property, label, fieldInfo);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.GetPropertyHeight(property, label);
    }

    [CustomPropertyDrawer(typeof(SetRuntimeSingle<>))]
    internal class SetRuntimeSingleDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => SOReferenceGUIHelper.VarRefGUI(SOType.RuntimeSingle, Access.Setter, "runtimeSingle", position, property, label, fieldInfo);
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.GetPropertyHeight(property, label);
    }
#endif
}

