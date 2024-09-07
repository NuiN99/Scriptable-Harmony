using System.Collections;
using UnityEngine;

namespace NuiN.NExtensions.Editor
{
    public abstract class KeyListGenerator : ScriptableObject
    {
        public abstract IEnumerable GetKeys(System.Type type);
    }
}