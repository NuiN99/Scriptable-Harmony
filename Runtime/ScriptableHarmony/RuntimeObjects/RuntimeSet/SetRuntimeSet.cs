using System;
using System.Collections;
using System.Collections.Generic;
using NuiN.ScriptableHarmony.Internal.Helpers;
using NuiN.ScriptableHarmony.Internal.Logging;
using NuiN.ScriptableHarmony.RuntimeSet.References.Base;
using Object = UnityEngine.Object;

namespace NuiN.ScriptableHarmony.References
{
    [Serializable]
    public class SetRuntimeSet<T> : RuntimeSetReference<T>, IEnumerable<T>
    {
        public List<T> Entities => runtimeSet.entities;
        
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Entities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Entities.GetEnumerator();
    
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
            SHLogger.LogAddRemove("Added Entity", SOType.RuntimeSet, itemObj.name, true, true, runtimeSet);
        }
        public void AddNoInvoke(T item)
        {
            if (item == null) return;
            Entities.Add(item);
            
            Object itemObj = item as Object;
            SHLogger.LogAddRemove("Added Entity", SOType.RuntimeSet, itemObj.name, true, false, runtimeSet);
        }
        
        public void Insert(T item, int index)
        {
            if (item == null) return;
            
            var oldItems = new List<T>(Entities);
            Entities.Insert(index, item);
            
            runtimeSet.onAddWithListWithOld?.Invoke(oldItems, Entities);
            runtimeSet.onAddWithOld?.Invoke(oldItems, item);
            runtimeSet.onAddWithList?.Invoke(Entities);
            runtimeSet.onAdd?.Invoke(item);
            
            Object itemObj = item as Object;
            SHLogger.LogAddRemove($"Inserted Entity | Index: {index}", SOType.RuntimeSet, itemObj.name, true, true, runtimeSet);
        }
        public void InsertNoInvoke(T item, int index)
        {
            if (item == null) return;
            Entities.Insert(index, item);
            
            Object itemObj = item as Object;
            SHLogger.LogAddRemove($"Inserted Entity | Index: {index}", SOType.RuntimeSet, itemObj.name, true, false, runtimeSet);
        }
    
        public void Remove(T item)
        {
            if(!Entities.Remove(item)) return;
            
            var oldItems = new List<T>(Entities);
            
            runtimeSet.onRemoveWithListWithOld?.Invoke(oldItems, Entities);
            runtimeSet.onRemoveWithOld?.Invoke(oldItems, item);
            runtimeSet.onRemoveWithList?.Invoke(Entities);
            runtimeSet.onRemove?.Invoke(item);
            
            Object itemObj = item as Object;
            SHLogger.LogAddRemove("Removed Entity", SOType.RuntimeSet, itemObj.name, false, true, runtimeSet);
        }
        public void RemoveNoInvoke(T item)
        {
            if(!Entities.Remove(item)) return;
            
            Object itemObj = item as Object;
            SHLogger.LogAddRemove("Removed Entity", SOType.RuntimeSet, itemObj.name, false, false, runtimeSet);
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
            SHLogger.LogAddRemove($"Removed Entity | Index: {index}", SOType.RuntimeSet, itemObj.name, false, true, runtimeSet);
        }
        public void RemoveAtNoInvoke(int index)
        {
            if (Entities[index] == null) return;
            Entities.RemoveAt(index);
            
            Object itemObj = Entities[index] as Object;
            SHLogger.LogAddRemove($"Removed Entity | Index: {index}", SOType.RuntimeSet, itemObj.name, false, false, runtimeSet);
        }
        
        public void Replace(IEnumerable<T> newList)
        {
            var oldList = new List<T>(Entities);
            runtimeSet.entities = new List<T>(newList);

            runtimeSet.onReplaceWithOld?.Invoke(oldList, Entities);
            runtimeSet.onReplace?.Invoke(Entities);
            
            SHLogger.LogReplacedCleared($"Replaced Set", SOType.RuntimeSet, oldList.Count, Entities.Count, true, runtimeSet);
        }
        public void ReplaceNoInvoke(List<T> newList)
        {
            SHLogger.LogReplacedCleared($"Replaced Set", SOType.RuntimeSet, Entities.Count, newList.Count, false, runtimeSet);

            runtimeSet.entities = new List<T>(newList);
        }
        
        public void Clear()
        {
            var oldList = new List<T>(Entities);
            Entities.Clear();

            runtimeSet.onClearWithOld?.Invoke(oldList);
            runtimeSet.onClear?.Invoke();
            
            SHLogger.LogReplacedCleared($"Cleared Set", SOType.RuntimeSet, oldList.Count, 0, false, runtimeSet);
        }
        public void ClearNoInvoke()
        {
            SHLogger.LogReplacedCleared($"Cleared Set", SOType.RuntimeSet, Entities.Count, 0, false, runtimeSet);

            runtimeSet.entities.Clear();
        }
    }
}