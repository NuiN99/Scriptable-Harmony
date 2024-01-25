using NuiN.ScriptableHarmony.Core;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Editor
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(GetDictionary<,>))]
    internal class GetDictionaryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.VarRefGUI(SOType.Dictionary, Access.Getter, "dictionary", position, property, label, fieldInfo);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => SOReferenceGUIHelper.GetPropertyHeight(property, label);
    }

    [CustomPropertyDrawer(typeof(SetDictionary<,>))]
    internal class SetDictionaryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => SOReferenceGUIHelper.VarRefGUI(SOType.Dictionary, Access.Setter, "dictionary", position, property, label, fieldInfo);
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.GetPropertyHeight(property, label);
    }
#endif
}

