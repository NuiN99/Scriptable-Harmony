using UnityEngine;
using NuiN.ScriptableHarmony.ListVariable.Base;

namespace NuiN.ScriptableHarmony.ListVariable.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/ListVariables/string",
        fileName = "New String ListVariable")]
    internal class StringListVariableSO : ScriptableListVariableBaseSO<string> { }
}