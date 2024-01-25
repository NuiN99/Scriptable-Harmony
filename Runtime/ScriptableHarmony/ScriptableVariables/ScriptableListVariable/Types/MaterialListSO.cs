using NuiN.ScriptableHarmony.Core;
using UnityEngine;

namespace NuiN.ScriptableHarmony
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/ListVariables/Material",
        fileName = "New Material ListVariable")]
    internal class MaterialListSO : ScriptableListSO<Material> { }
}