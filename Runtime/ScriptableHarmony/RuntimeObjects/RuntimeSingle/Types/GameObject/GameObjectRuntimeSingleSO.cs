using UnityEngine;
using NuiN.ScriptableHarmony.RuntimeSingle.Base;

namespace NuiN.ScriptableHarmony.RuntimeSingle.Common
{
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/RuntimeSingles/GameObject", 
        fileName = "New GameObject RuntimeSingle")]
    internal class GameObjectRuntimeSingleSO : RuntimeSingleSO<GameObject> { }
}

