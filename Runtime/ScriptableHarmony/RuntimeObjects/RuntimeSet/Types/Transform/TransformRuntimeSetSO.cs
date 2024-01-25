using NuiN.ScriptableHarmony.RuntimeSet.Base;
using UnityEngine;

namespace NuiN.ScriptableHarmony.RuntimeSet.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/RuntimeSets/Transform", 
        fileName = "New Transform RuntimeSet")]
    internal class TransformRuntimeSetSO : RuntimeSetSO<Transform> { }
}