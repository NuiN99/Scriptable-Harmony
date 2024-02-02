using NuiN.ScriptableHarmony.Core;
using UnityEngine;

namespace NuiN.ScriptableHarmony
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/Variables/Material", 
        fileName = "New Material Variable")]
    internal class MaterialVariableSO : ScriptableVariableSO<Material> { }
}