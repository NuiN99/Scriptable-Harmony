using NuiN.ScriptableHarmony.Core;
using UnityEngine;

namespace NuiN.ScriptableHarmony
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/RuntimeSingles/Rigidbody", 
        fileName = "New Rigidbody RuntimeSingle")]
    internal class RigidbodyRuntimeSingleSO : RuntimeSingleSO<Rigidbody> { }
}