using NuiN.ScriptableHarmony.Core;
using UnityEditor;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Editor
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(GetScriptableDictionary<,>))]
    internal class GetDictionaryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.DrawGUI(SOType.Dictionary, Access.Getter, "dictionary", position, property, label, fieldInfo);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => SOReferenceGUIHelper.GetPropertyHeight(property, label);
    }

    [CustomPropertyDrawer(typeof(SetScriptableDictionary<,>))]
    internal class SetDictionaryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            => SOReferenceGUIHelper.DrawGUI(SOType.Dictionary, Access.Setter, "dictionary", position, property, label, fieldInfo);
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => SOReferenceGUIHelper.GetPropertyHeight(property, label);
    }
#endif
}

