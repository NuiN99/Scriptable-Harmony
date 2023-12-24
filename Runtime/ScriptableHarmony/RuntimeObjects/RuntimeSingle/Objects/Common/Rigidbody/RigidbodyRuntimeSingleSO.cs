using UnityEngine;
using NuiN.ScriptableHarmony.RuntimeSingle.Base;

namespace NuiN.ScriptableHarmony.RuntimeSingle.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/RuntimeSingles/Rigidbody", 
        fileName = "New Rigidbody RuntimeSingle")]
    internal class RigidbodyRuntimeSingleSO : RuntimeSingleBaseSO<Rigidbody> { }
}