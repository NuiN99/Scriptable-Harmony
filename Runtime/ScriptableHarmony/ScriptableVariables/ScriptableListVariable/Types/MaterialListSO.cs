using UnityEngine;
using NuiN.ScriptableHarmony.ListVariable.Base;

namespace NuiN.ScriptableHarmony.ListVariable.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/ListVariables/Material",
        fileName = "New Material ListVariable")]
    internal class MaterialListSO : ScriptableListSO<Material> { }
}