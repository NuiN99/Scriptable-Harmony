using UnityEngine;
using NuiN.ScriptableHarmony.ListVariable.Base;

namespace NuiN.ScriptableHarmony.ListVariable.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/ListVariables/Color",
        fileName = "New Color ListVariable")]
    internal class ColorListSO : ScriptableListSO<Color> { }
}