using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NuiN.ScriptableHarmony.Core;
using UnityEditor;

namespace NuiN.ScriptableHarmony
{
    [Serializable]
    public class SetScriptableList<T> : ScriptableListReference<T>, IEnumerable<T>
    {
        public List<T> Items => list.values;
        
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
        
        public bool Contains(T item) => Items.Contains(item);
        public int Count => Items.Count;
        public T this[int index] => Items[index];
        
        public void Add(T item)
        {
            var oldValue = new List<T>(Items);
            Items.Add(item);
            SetDirty();

            list.onAddWithListWithOld?.Invoke(oldValue, Items);
            list.onAddWithOld?.Invoke(oldValue, item);
            list.onAddWithList?.Invoke(Items);
            list.onAdd?.Invoke(item);
            
            SHLogger.LogAddRemove("Added Item", SOType.List, item?.ToString(), true, list);
        }

        public void Insert(T item, int index)
        {
            var oldValue = new List<T>(Items);
            Items.Insert(index, item);
            SetDirty();

            list.onAddWithListWithOld?.Invoke(oldValue, Items);
            list.onAddWithOld?.Invoke(oldValue, item);
            list.onAddWithList?.Invoke(Items);
            list.onAdd?.Invoke(item);
            
            SHLogger.LogAddRemove($"Inserted Item | Index: {index}", SOType.List, item?.ToString(), true, list);
        }

        public void RemoveAt(int index)
        {
            var oldValue = new List<T>(Items);

            T removedItem = Items[index];
            Items.RemoveAt(index);
            SetDirty();

            list.onRemoveWithListWithOld?.Invoke(oldValue, Items);
            list.onRemoveWithOld?.Invoke(oldValue, removedItem);
            list.onRemoveWithList?.Invoke(Items);
            list.onRemove?.Invoke(removedItem);
            
            SHLogger.LogAddRemove($"Removed Item | Index: {index}", SOType.List, removedItem?.ToString(), true, list);
        }
        
        public void Remove(T item)
        {
            var oldValue = new List<T>(Items);
            Items.Remove(item);
            SetDirty();

            list.onRemoveWithListWithOld?.Invoke(oldValue, Items);
            list.onRemoveWithOld?.Invoke(oldValue, item);
            list.onRemoveWithList?.Invoke(Items);
            list.onRemove?.Invoke(item);
            
            SHLogger.LogAddRemove("Removed Item", SOType.List, item?.ToString(), true, list);
        }
        
        public void Replace(IEnumerable<T> newList)
        {
            var oldValue = new List<T>(Items);
            list.values = new List<T>(newList);
            SetDirty();

            list.onReplaceWithOld?.Invoke(oldValue, Items);
            list.onReplace?.Invoke(Items);
            
            SHLogger.LogReplacedCleared("Replaced List", SOType.List, oldValue.Count, Items.Count, list);
        }
        
        public void Clear()
        {
            var oldValue = new List<T>(Items);
            Items.Clear();
            SetDirty();

            list.onClearWithOld?.Invoke(oldValue);
            list.onClear?.Invoke();
            
            SHLogger.LogReplacedCleared("Cleared List", SOType.List, oldValue.Count, 0, list);
        }
        
        public void ResetValues()
        {
            Replace(list.DefaultValues);
        }

        [Conditional("UNITY_EDITOR")]
        void SetDirty()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(list);
#endif
        }
    }
}
