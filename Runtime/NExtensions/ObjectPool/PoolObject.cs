using System;
using UnityEngine;

namespace NuiN.NExtensions
{
    public class PoolObject : MonoBehaviour, IPoolabeObject<PoolObject>
    {
        public Action<PoolObject> ReleaseToPool { get; set; }
    
        void OnDisable()
        {
            ReleaseToPool?.Invoke(this);
        }
    }
}
