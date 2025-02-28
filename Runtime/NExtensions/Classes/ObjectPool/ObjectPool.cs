using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


namespace NuiN.NExtensions
{
    /// <summary> Stack based Object Pool that allows objects to define when to release themselves back into the pool, without passing the pool around </summary>
    public class ObjectPool<T> : IDisposable where T : MonoBehaviour
    {
        Stack<T> _stack = new();
        Func<T> _create;
        Action<T> _onGet;
        Action<T> _onRelease;
        bool _autoEnableDisable;

        bool _useContainer;
        Transform _container;
        
        /// <param name="create"> How the object gets created, eg. () => Instantiate(prefab) </param>
        /// <param name="onGet"> When getting the object </param>
        /// <param name="onRelease"> When the object is released back into the pool </param>
        /// <param name="preloadCount"> How many objects to spawn when the pool is initialized </param>
        /// <param name="autoEnableDisable"> Automatically enable on get, and disable on release </param>
        public ObjectPool(Func<T> create = null, Action<T> onGet = null, Action<T> onRelease = null, int preloadCount = 0, bool autoEnableDisable = true)
        {
            _onGet = onGet;
            _onRelease = onRelease;
            _create = create;
            _autoEnableDisable = autoEnableDisable;

            for (int i = 0; i < preloadCount; i++)
            {
                Create();
            }
        }

        public void CreateContainer(string name)
        {
            _useContainer = true;
            _container = new GameObject(name).transform;

            foreach (var obj in _stack)
            {
                obj.transform.SetParent(_container);
            }
        }
    
        /// <summary> Get an object from the pool </summary>
        public T Get()
        {
            T obj = _stack.Count == 0 ? Create() : _stack.Pop();
            
            if(_autoEnableDisable) obj.gameObject.SetActive(true);

            _onGet?.Invoke(obj);
            
            return obj;
        }

        /// <summary> Return an object back into the pool </summary>
        public void Release(T obj)
        {
            _stack.Push(obj);
            
            if(_autoEnableDisable) obj.gameObject.SetActive(false);
            if(_useContainer) obj.transform.SetParent(_container);
        
            _onRelease?.Invoke(obj);
        }

        /// <summary> Create and initialize an object </summary>
        T Create()
        {
            T obj = _create.Invoke();

            if(_autoEnableDisable) obj.gameObject.SetActive(false);
            if(_useContainer) obj.transform.SetParent(_container);
            
            _stack.Push(obj);
            
            if(obj is IPoolabeObject<T> poolable) 
                poolable.ReleaseToPool = Release;
            
            return obj;
        }

        /// <summary> IDisposable interface implementation </summary>
        public void Dispose()
        {
            foreach (var obj in _stack)
            {
                Object.Destroy(obj.gameObject);
            }
            
            _stack.Clear();
        }
    }
}
