#if UNITY_EDITOR

using UnityEditor;

namespace NuiN.NExtensions.Editor
{
    public class PropertySearchResult
    {
        public SerializedProperty Property;

        public PropertySearchResult(SerializedProperty property)
        {
            Property = property;
        }

        public override string ToString()
        {
            return $"Found match in in {Property.propertyPath}";
        }
    }
}
#endif