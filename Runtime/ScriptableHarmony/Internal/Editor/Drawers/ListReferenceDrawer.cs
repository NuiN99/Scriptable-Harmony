using NuiN.ScriptableHarmony.Core;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Editor
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(GetList<>))]
    internal class GetListVariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.VarRefGUI(SOType.List, Access.Getter, "list", position, property, label, fieldInfo);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.GetPropertyHeight(property, label);
    }

    [CustomPropertyDrawer(typeof(SetListVariable<>))]
    internal class SetListVariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => SOReferenceGUIHelper.VarRefGUI(SOType.List, Access.Setter, "list", position, property, label, fieldInfo);
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.GetPropertyHeight(property, label);
    }
#endif
}

