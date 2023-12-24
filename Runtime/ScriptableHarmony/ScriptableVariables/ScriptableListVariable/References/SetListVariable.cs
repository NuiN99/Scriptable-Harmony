using System;
using System.Collections.Generic;
using System.Diagnostics;
using NuiN.ScriptableHarmony.Internal.Helpers;
using NuiN.ScriptableHarmony.Internal.Logging;
using NuiN.ScriptableHarmony.ListVariable.Base;
using NuiN.ScriptableHarmony.ListVariable.References.Base;
using UnityEditor;

namespace NuiN.ScriptableHarmony.References
{
    [Serializable]
    public class SetListVariable<T> : ReferenceScriptableListVariableBase<T>
    {
        public List<T> Values => list.values;
        
        public void Add(T item)
        {
            var oldValue = new List<T>(Values);
            Values.Add(item);
            SetDirty();

            list.onAddWithListWithOld?.Invoke(oldValue, Values);
            list.onAddWithOld?.Invoke(oldValue, item);
            list.onAddWithList?.Invoke(Values);
            list.onAdd?.Invoke(item);
            
            SHLogger.LogAddRemove("Added Item", SOType.ListVariable, item?.ToString(), true, true, list);
        }
        public void AddNoInvoke(T item)
        {
            Values.Add(item);
            SetDirty();
            
            SHLogger.LogAddRemove("Added Item", SOType.ListVariable, item?.ToString(), true, false, list);
        }

        public void Insert(T item, int index)
        {
            var oldValue = new List<T>(Values);
            Values.Insert(index, item);
            SetDirty();

            list.onAddWithListWithOld?.Invoke(oldValue, Values);
            list.onAddWithOld?.Invoke(oldValue, item);
            list.onAddWithList?.Invoke(Values);
            list.onAdd?.Invoke(item);
            
            SHLogger.LogAddRemove($"Inserted Item | Index: {index}", SOType.ListVariable, item?.ToString(), true, true, list);
        }
        public void InsertNoInvoke(T item, int index)
        {
            Values.Insert(index, item);
            SetDirty();
            
            SHLogger.LogAddRemove($"Inserted Item | Index: {index}", SOType.ListVariable, item?.ToString(), true, false, list);
        }

        public void RemoveAt(int index)
        {
            var oldValue = new List<T>(Values);

            T removedItem = Values[index];
            Values.RemoveAt(index);
            SetDirty();

            list.onRemoveWithListWithOld?.Invoke(oldValue, Values);
            list.onRemoveWithOld?.Invoke(oldValue, removedItem);
            list.onRemoveWithList?.Invoke(Values);
            list.onRemove?.Invoke(removedItem);
            
            SHLogger.LogAddRemove($"Removed Item | Index: {index}", SOType.ListVariable, removedItem?.ToString(), true, true, list);
        }
        public void RemoveAtNoInvoke(int index)
        {
            SHLogger.LogAddRemove($"Removed Item | Index: {index}", SOType.ListVariable, Values[index]?.ToString(), true, false, list);
            
            Values.RemoveAt(index);
            SetDirty();
        }
        
        public void Remove(T item)
        {
            var oldValue = new List<T>(Values);
            Values.Remove(item);
            SetDirty();

            list.onRemoveWithListWithOld?.Invoke(oldValue, Values);
            list.onRemoveWithOld?.Invoke(oldValue, item);
            list.onRemoveWithList?.Invoke(Values);
            list.onRemove?.Invoke(item);
            
            SHLogger.LogAddRemove("Removed Item", SOType.ListVariable, item?.ToString(), true, true, list);
        }
        public void RemoveNoInvoke(T item)
        {
            Values.Remove(item);
            SetDirty();
            
            SHLogger.LogAddRemove("Removed Item", SOType.ListVariable, item?.ToString(), true, false, list);
        }
        
        public void Replace(IEnumerable<T> newList)
        {
            var oldValue = new List<T>(Values);
            list.values = new List<T>(newList);
            SetDirty();

            list.onReplaceWithOld?.Invoke(oldValue, Values);
            list.onReplace?.Invoke(Values);
            
            SHLogger.LogReplacedCleared("Replaced List", SOType.ListVariable, oldValue.Count, Values.Count, true, list);
        }
        public void ReplaceNoInvoke(List<T> newList)
        {
            SHLogger.LogReplacedCleared("Replaced List", SOType.ListVariable, Values.Count, newList.Count, false, list);

            list.values = new List<T>(newList);
            SetDirty();
        }
        
        public void Clear()
        {
            var oldValue = new List<T>(Values);
            Values.Clear();
            SetDirty();

            list.onClearWithOld?.Invoke(oldValue);
            list.onClear?.Invoke();
            
            SHLogger.LogReplacedCleared("Cleared List", SOType.ListVariable, oldValue.Count, 0, true, list);
        }
        public void ClearNoInvoke()
        {
            SHLogger.LogReplacedCleared("Cleared List", SOType.ListVariable, Values.Count, 0, true, list);

            Values.Clear();
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
