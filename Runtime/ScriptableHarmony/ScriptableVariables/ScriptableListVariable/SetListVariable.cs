using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NuiN.ScriptableHarmony.Internal.Helpers;
using NuiN.ScriptableHarmony.Internal.Logging;
using NuiN.ScriptableHarmony.ListVariable.References.Base;
using UnityEditor;

namespace NuiN.ScriptableHarmony.References
{
    [Serializable]
    public class SetListVariable<T> : ScriptableListReference<T>, IEnumerable<T>
    {
        public List<T> Items => list.values;
        
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
        
        public void Add(T item)
        {
            var oldValue = new List<T>(Items);
            Items.Add(item);
            SetDirty();

            list.onAddWithListWithOld?.Invoke(oldValue, Items);
            list.onAddWithOld?.Invoke(oldValue, item);
            list.onAddWithList?.Invoke(Items);
            list.onAdd?.Invoke(item);
            
            SHLogger.LogAddRemove("Added Item", SOType.List, item?.ToString(), true, true, list);
        }
        public void AddNoInvoke(T item)
        {
            Items.Add(item);
            SetDirty();
            
            SHLogger.LogAddRemove("Added Item", SOType.List, item?.ToString(), true, false, list);
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
            
            SHLogger.LogAddRemove($"Inserted Item | Index: {index}", SOType.List, item?.ToString(), true, true, list);
        }
        public void InsertNoInvoke(T item, int index)
        {
            Items.Insert(index, item);
            SetDirty();
            
            SHLogger.LogAddRemove($"Inserted Item | Index: {index}", SOType.List, item?.ToString(), true, false, list);
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
            
            SHLogger.LogAddRemove($"Removed Item | Index: {index}", SOType.List, removedItem?.ToString(), true, true, list);
        }
        public void RemoveAtNoInvoke(int index)
        {
            SHLogger.LogAddRemove($"Removed Item | Index: {index}", SOType.List, Items[index]?.ToString(), true, false, list);
            
            Items.RemoveAt(index);
            SetDirty();
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
            
            SHLogger.LogAddRemove("Removed Item", SOType.List, item?.ToString(), true, true, list);
        }
        public void RemoveNoInvoke(T item)
        {
            Items.Remove(item);
            SetDirty();
            
            SHLogger.LogAddRemove("Removed Item", SOType.List, item?.ToString(), true, false, list);
        }
        
        public void Replace(IEnumerable<T> newList)
        {
            var oldValue = new List<T>(Items);
            list.values = new List<T>(newList);
            SetDirty();

            list.onReplaceWithOld?.Invoke(oldValue, Items);
            list.onReplace?.Invoke(Items);
            
            SHLogger.LogReplacedCleared("Replaced List", SOType.List, oldValue.Count, Items.Count, true, list);
        }
        public void ReplaceNoInvoke(List<T> newList)
        {
            SHLogger.LogReplacedCleared("Replaced List", SOType.List, Items.Count, newList.Count, false, list);

            list.values = new List<T>(newList);
            SetDirty();
        }
        
        public void Clear()
        {
            var oldValue = new List<T>(Items);
            Items.Clear();
            SetDirty();

            list.onClearWithOld?.Invoke(oldValue);
            list.onClear?.Invoke();
            
            SHLogger.LogReplacedCleared("Cleared List", SOType.List, oldValue.Count, 0, true, list);
        }
        public void ClearNoInvoke()
        {
            SHLogger.LogReplacedCleared("Cleared List", SOType.List, Items.Count, 0, true, list);

            Items.Clear();
            SetDirty();
        }

        public void ResetValues()
        {
            Replace(list.DefaultValues);
        }
        public void ResetValuesNoInvoke()
        {
            ReplaceNoInvoke(list.DefaultValues);
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
