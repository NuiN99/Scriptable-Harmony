using UnityEngine;
using NuiN.ScriptableHarmony.RuntimeSingle.Base;

namespace NuiN.ScriptableHarmony.RuntimeSingle.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/RuntimeSingles/Rigidbody2D", 
        fileName = "New Rigidbody2D RuntimeSingle")]
    internal class Rigidbody2DRuntimeSingleSO : RuntimeSingleBaseSO<Rigidbody2D> { }
}