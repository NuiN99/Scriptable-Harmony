using NuiN.ScriptableHarmony.Core.Editor.Helpers;
using NuiN.ScriptableHarmony.Internal.Helpers;
using NuiN.ScriptableHarmony.References;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Internal.Editor
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(GetDictionary<,>))]
    internal class GetDictionaryVariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.VarRefGUI(SOType.Dictionary, Access.Getter, "dictionary", position, property, label, fieldInfo);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.GetPropertyHeight(property, label);
    }

    [CustomPropertyDrawer(typeof(SetDictionary<,>))]
    internal class SetDictionaryVariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => SOReferenceGUIHelper.VarRefGUI(SOType.Dictionary, Access.Setter, "dictionary", position, property, label, fieldInfo);
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.GetPropertyHeight(property, label);
    }
#endif
}

