using NuiN.ScriptableHarmony.Core;
using UnityEngine;

namespace NuiN.ScriptableHarmony
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/ListVariables/string",
        fileName = "New String ListVariable")]
    internal class StringListSO : ScriptableListSO<string> { }
}