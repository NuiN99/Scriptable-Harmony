using UnityEngine;
using NuiN.ScriptableHarmony.RuntimeSingle.Base;

namespace NuiN.ScriptableHarmony.RuntimeSingle.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/RuntimeSingles/Transform", 
        fileName = "New Transform RuntimeSingle")]
    internal class TransformRuntimeSingleSO : RuntimeSingleSO<Transform> { }
}