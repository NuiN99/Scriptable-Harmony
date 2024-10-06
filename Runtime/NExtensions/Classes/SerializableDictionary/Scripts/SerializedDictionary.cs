using System.Collections.Generic;
using UnityEngine;

namespace NuiN.NExtensions
{
    // https://github.com/ayellowpaper/SerializedDictionary
    [System.Serializable]
    public partial class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        internal List<SerializedKeyValuePair<TKey, TValue>> serializedList;
        
        public SerializedDictionary(SerializedDictionary<TKey, TValue> dictionary)
        {
            serializedList = new List<SerializedKeyValuePair<TKey, TValue>>();
            Clear();
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                serializedList.Add(new SerializedKeyValuePair<TKey, TValue>(pair.Key, pair.Value));
                TryAdd(pair.Key, pair.Value);
            }
        }

#if UNITY_EDITOR
        internal IKeyable LookupTable
        {
            get
            {
                if (_lookupTable == null)
                    _lookupTable = new DictionaryLookupTable<TKey, TValue>(this);
                return _lookupTable;
            }
        }

        private DictionaryLookupTable<TKey, TValue> _lookupTable;
#endif

        public void OnAfterDeserialize()
        {
            Clear();

            foreach (var kvp in serializedList)
            {
#if UNITY_EDITOR
                if (!ContainsKey(kvp.Key))
                    Add(kvp.Key, kvp.Value);
#else
                    Add(kvp.Key, kvp.Value);
#endif
            }

#if UNITY_EDITOR
            LookupTable.RecalculateOccurences();
#else
            serializedList.Clear();
#endif
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
                LookupTable.RemoveDuplicates();
#endif
        }
    }
}
