using UnityEngine;
using NuiN.ScriptableHarmony.ListVariable.Base;

namespace NuiN.ScriptableHarmony.ListVariable.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/ListVariables/int", 
        fileName = "New Int ListVariable")]
    internal class IntListSO : ScriptableListSO<int> { }
}