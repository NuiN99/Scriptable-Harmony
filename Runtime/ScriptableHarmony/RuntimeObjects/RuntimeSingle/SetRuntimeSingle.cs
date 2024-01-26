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

        void Set(T newItem, bool overrideExisting)
        {
            if (newItem == null) return;
            if (Entity != null && !overrideExisting) return;

            T oldItem = Entity;
            Entity = newItem;

            Object newItemObj = newItem as Object;
            Object oldItemObj = oldItem as Object;
            SHLogger.LogSet("Set Entity", SOType.RuntimeSingle, oldItemObj != null ? oldItemObj.name : "", newItemObj != null ? newItemObj.name : "", runtimeSingle);
            
            runtimeSingle.onSetWithOld?.Invoke(oldItem, Entity);
            runtimeSingle.onSet?.Invoke(Entity);
        }
        public void TrySet(T newItem)
            => Set(newItem, false);
        public void Set(T newItem)
            => Set(newItem, true);
        
        public void Remove()
        {
            if (Entity == null) return;

            T oldItem = Entity;
            Entity = default;
            
            Object oldItemObj = oldItem as Object;
            if (oldItemObj != null) SHLogger.LogSet("Removed Entity", SOType.RuntimeSingle, oldItem != null ? oldItemObj.name : "null", null, runtimeSingle);

            runtimeSingle.onRemoveWithOld?.Invoke(oldItem);
            runtimeSingle.onRemove?.Invoke();
        }
    }
}