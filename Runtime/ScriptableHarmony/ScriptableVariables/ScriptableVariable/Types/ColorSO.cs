using NuiN.ScriptableHarmony.Variable.Base;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Variable.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/Variables/Color", 
        fileName = "New Color Variable")]
    internal class ColorSO : ScriptableVariableSO<Color> { }
}