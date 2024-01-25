using System;
using NuiN.ScriptableHarmony.Core;
using Object = UnityEngine.Object;

namespace NuiN.ScriptableHarmony
{
    [Serializable]
    public class SetRuntimeSingle<T> : RuntimeSingleReference<T>
    {
        public T Entity
        {
            get => runtimeSingle.entity;
            private set => runtimeSingle.entity = value;
        }

        void Set(T newItem, bool overrideExisting, bool invokeActions)
        {
            if (newItem == null) return;
            if (Entity != null && !overrideExisting) return;

            T oldItem = Entity;
            Entity = newItem;

            Object newItemObj = newItem as Object;
            Object oldItemObj = oldItem as Object;
            SHLogger.LogSet("Set Entity", SOType.RuntimeSingle, oldItemObj != null ? oldItemObj.name : "", newItemObj != null ? newItemObj.name : "", invokeActions, runtimeSingle);
            
            if (!invokeActions) return;

            runtimeSingle.onSetWithOld?.Invoke(oldItem, Entity);
            runtimeSingle.onSet?.Invoke(Entity);
        }
        public void TrySet(T newItem)
            => Set(newItem, false, true);
        public void TrySetNoInvoke(T newItem)
            => Set(newItem, false, false);
        public void Set(T newItem)
            => Set(newItem, true, true);
        public void SetNoInvoke(T newItem, bool invokeActions = true, bool overrideExisting = true)
            => Set(newItem, true, false);
        
        void Remove(bool invokeActions)
        {
            if (Entity == null) return;

            T oldItem = Entity;
            Entity = default;
            
            Object oldItemObj = oldItem as Object;
            SHLogger.LogSet("Removed Entity", SOType.RuntimeSingle, oldItem != null ? oldItemObj.name : "null", null, invokeActions, runtimeSingle);
            
            if (!invokeActions) return;
            
            runtimeSingle.onRemoveWithOld?.Invoke(oldItem);
            runtimeSingle.onRemove?.Invoke();
        }
        public void Remove() => Remove(true);
        public void RemoveNoInvoke() => Remove(false);
    }
}