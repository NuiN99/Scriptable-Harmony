using NuiN.ScriptableHarmony.Core;
using UnityEngine;

namespace NuiN.ScriptableHarmony
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/Variables/Color", 
        fileName = "New Color Variable")]
    internal class ColorSO : ScriptableVariableSO<Color> { }
}