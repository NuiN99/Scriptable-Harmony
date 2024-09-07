using UnityEngine;

namespace NuiN.NExtensions.Editor
{
    [System.Serializable]
    internal class PropertyData
    {
        [SerializeField]
        private ElementData _keyData;
        [SerializeField]
        private ElementData _valueData;
        [SerializeField]
        private bool _alwaysShowSearch = false;

        public bool AlwaysShowSearch
        {
            get => _alwaysShowSearch;
            set => _alwaysShowSearch = value;
        }

        public ElementData GetElementData(bool fieldType)
        {
            return fieldType == SCEditorUtility.KeyFlag ? _keyData : _valueData;
        }

        public PropertyData() : this(new ElementSettings(), new ElementSettings()) { }

        public PropertyData(ElementSettings keySettings, ElementSettings valueSettings)
        {
            _keyData = new ElementData(keySettings);
            _valueData = new ElementData(valueSettings);
        }
    }
}