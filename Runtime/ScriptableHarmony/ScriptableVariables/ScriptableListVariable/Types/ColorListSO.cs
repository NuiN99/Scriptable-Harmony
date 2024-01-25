using NuiN.ScriptableHarmony.Core;
using UnityEngine;

namespace NuiN.ScriptableHarmony
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/ListVariables/Color",
        fileName = "New Color ListVariable")]
    internal class ColorListSO : ScriptableListSO<Color> { }
}