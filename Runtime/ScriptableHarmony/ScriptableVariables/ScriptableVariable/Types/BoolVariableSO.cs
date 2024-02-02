using NuiN.ScriptableHarmony.Core;
using UnityEngine;

namespace NuiN.ScriptableHarmony
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/Variables/bool", 
        fileName = "New Bool Variable")]
    internal class BoolVariableSO : ScriptableVariableSO<bool> { }
}