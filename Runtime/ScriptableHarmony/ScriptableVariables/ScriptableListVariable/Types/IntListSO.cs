using NuiN.ScriptableHarmony.Core;
using UnityEngine;

namespace NuiN.ScriptableHarmony
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/ListVariables/int", 
        fileName = "New Int ListVariable")]
    internal class IntListSO : ScriptableListSO<int> { }
}