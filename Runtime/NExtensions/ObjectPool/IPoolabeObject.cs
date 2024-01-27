using System;
using UnityEngine;

namespace NuiN.NExtensions
{
    public interface IPoolabeObject<T> where T : MonoBehaviour
    {
        Action<T> ReleaseToPool { get; set; }
    }
}
