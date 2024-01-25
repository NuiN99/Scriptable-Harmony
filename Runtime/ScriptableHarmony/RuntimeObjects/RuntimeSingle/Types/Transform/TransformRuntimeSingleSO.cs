using NuiN.ScriptableHarmony.Core;
using UnityEngine;

namespace NuiN.ScriptableHarmony
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/RuntimeSingles/Transform", 
        fileName = "New Transform RuntimeSingle")]
    internal class TransformRuntimeSingleSO : RuntimeSingleSO<Transform> { }
}