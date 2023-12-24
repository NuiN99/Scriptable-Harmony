using NuiN.ScriptableHarmony.Variable.Base;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Variable.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/Variables/string", 
        fileName = "New String Variable")]
    internal class StringVariableSO : ScriptableVariableBaseSO<string> { }
}