using System;
using System.Collections;
using System.Collections.Generic;
using NuiN.ScriptableHarmony.Core;
using Object = UnityEngine.Object;

namespace NuiN.ScriptableHarmony
{
    [Serializable]
    public class SetRuntimeSet<T> : RuntimeSetReference<T>, IEnumerable<T>
    {
        public List<T> Entities => runtimeSet.entities;
        
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Entities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Entities.GetEnumerator();

        public bool Contains(T item) => Entities.Contains(item);
        public int Count => Entities.Count;
        public T this[int index] => Entities[index];

        public void Add(T item)
        {
            if (item == null) return;
        
            var oldItems = new List<T>(Entities);
            Entities.Add(item);
            
            runtimeSet.onAddWithListWithOld?.Invoke(oldItems, Entities);
            runtimeSet.onAddWithOld?.Invoke(oldItems, item);
            runtimeSet.onAddWithList?.Invoke(Entities);
            runtimeSet.onAdd?.Invoke(item);

            Object itemObj = item as Object;
            if (itemObj != null) SHLogger.LogAddRemove("Added Entity", SOType.RuntimeSet, itemObj.name, true, runtimeSet);
        }
        
        public void Insert(int index, T item)
        {
            if (item == null) return;
            
            var oldItems = new List<T>(Entities);
            Entities.Insert(index, item);
            
            runtimeSet.onAddWithListWithOld?.Invoke(oldItems, Entities);
            runtimeSet.onAddWithOld?.Invoke(oldItems, item);
            runtimeSet.onAddWithList?.Invoke(Entities);
            runtimeSet.onAdd?.Invoke(item);
            
            Object itemObj = item as Object;
            if (itemObj != null) SHLogger.LogAddRemove($"Inserted Entity | Index: {index}", SOType.RuntimeSet, itemObj.name, true, runtimeSet);
        }
        
        public bool Remove(T item)
        {
            if(!Entities.Remove(item)) return false;
            
            var oldItems = new List<T>(Entities);
            
            runtimeSet.onRemoveWithListWithOld?.Invoke(oldItems, Entities);
            runtimeSet.onRemoveWithOld?.Invoke(oldItems, item);
            runtimeSet.onRemoveWithList?.Invoke(Entities);
            runtimeSet.onRemove?.Invoke(item);
            
            Object itemObj = item as Object;
            if (itemObj != null) SHLogger.LogAddRemove("Removed Entity", SOType.RuntimeSet, itemObj.name, false, runtimeSet);

            return true;
        }
        
        public void RemoveAt(int index)
        {
            T removedItem = Entities[index];
            if (removedItem == null) return;
            
            var oldItems = new List<T>(Entities);
            Entities.RemoveAt(index);
            
            runtimeSet.onRemoveWithListWithOld?.Invoke(oldItems, Entities);
            runtimeSet.onRemoveWithOld?.Invoke(oldItems, removedItem);
            runtimeSet.onRemoveWithList?.Invoke(Entities);
            runtimeSet.onRemove?.Invoke(removedItem);
            
            Object itemObj = removedItem as Object;
            if (itemObj != null) SHLogger.LogAddRemove($"Removed Entity | Index: {index}", SOType.RuntimeSet, itemObj.name, false, runtimeSet);
        }

        public void Replace(IEnumerable<T> newList)
        {
            var oldList = new List<T>(Entities);
            runtimeSet.entities = new List<T>(newList);

            runtimeSet.onReplaceWithOld?.Invoke(oldList, Entities);
            runtimeSet.onReplace?.Invoke(Entities);
            
            SHLogger.LogReplacedCleared($"Replaced Set", SOType.RuntimeSet, oldList.Count, Entities.Count, runtimeSet);
        }
        
        public void Clear()
        {
            var oldList = new List<T>(Entities);
            Entities.Clear();

            runtimeSet.onClearWithOld?.Invoke(oldList);
            runtimeSet.onClear?.Invoke();
            
            SHLogger.LogReplacedCleared($"Cleared Set", SOType.RuntimeSet, oldList.Count, 0, runtimeSet);
        }
    }
}