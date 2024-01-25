using NuiN.ScriptableHarmony.Core;
using UnityEngine;

namespace NuiN.ScriptableHarmony.RuntimeSet.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/RuntimeSets/Transform", 
        fileName = "New Transform RuntimeSet")]
    internal class TransformRuntimeSetSO : RuntimeSetSO<Transform> { }
}