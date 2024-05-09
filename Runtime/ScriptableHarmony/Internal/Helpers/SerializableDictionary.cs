using System;
using System.Collections.Generic;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Core
{
   [Serializable]
   public class SerializableDictionary<TKey,TValue>
   {
      [NonSerialized] public Dictionary<TKey, TValue> dictionary;
      [SerializeField] List<SerializedKeyValuePair<TKey, TValue>> serializedPairs = new();
   
      public SerializableDictionary(ref Dictionary<TKey, TValue> dictionary)
      {
         this.dictionary = dictionary;
      }
      
      public void Add(TKey key, TValue value)
      {
         serializedPairs.Add(new SerializedKeyValuePair<TKey, TValue>(key, value));
      }
      public void Serialize(ref Dictionary<TKey, TValue> newDict)
      {
         dictionary = newDict;
         serializedPairs?.Clear();
         foreach (KeyValuePair<TKey, TValue> item in dictionary)
         {
            Add(item.Key, item.Value);
         }
      }
      
      public void ValidateAndApply(ref Dictionary<TKey, TValue> newDict)
      {
         dictionary = newDict;
         
         dictionary.Clear();
         List<SerializedKeyValuePair<TKey, TValue>> duplicates = new();
         foreach (SerializedKeyValuePair<TKey, TValue> pair in serializedPairs)
         {
            bool success = dictionary.TryAdd(pair.key, pair.value);
            if(!success) duplicates.Add(pair);
         }
   
         foreach (SerializedKeyValuePair<TKey, TValue> duplicate in duplicates)
         {
            LogDuplicateWarning(duplicate);
            serializedPairs.Remove(duplicate);
         }
         Serialize(ref dictionary);
      }

      public void SetValues(ref Dictionary<TKey, TValue> dictionary)
      {
         serializedPairs.Clear();
         foreach (KeyValuePair<TKey, TValue> pair in dictionary)
         {
            serializedPairs.Add(new SerializedKeyValuePair<TKey, TValue>(pair.Key, pair.Value));
         }

         this.dictionary = new Dictionary<TKey, TValue>();
         ValidateAndApply(ref this.dictionary);
      }
   
      public Dictionary<TKey, TValue> GetDictionary()
      {
         var dict = new Dictionary<TKey, TValue>();
         foreach (SerializedKeyValuePair<TKey, TValue> pair in serializedPairs)
         {
            if (!dict.TryAdd(pair.key, pair.value) && Application.isPlaying) LogDuplicateWarning(pair);
         }
         return dict;
      }
   
      void LogDuplicateWarning(SerializedKeyValuePair<TKey, TValue> pair)
      {
         Debug.LogWarning($"Dictionary Validation: Removed duplicate | Key: {pair.key} | Value: {pair.value}");
      }
   }
}


