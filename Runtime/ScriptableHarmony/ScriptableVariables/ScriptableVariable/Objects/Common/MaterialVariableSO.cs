using NuiN.ScriptableHarmony.Variable.Base;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Variable.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/Variables/Material", 
        fileName = "New Material Variable")]
    internal class MaterialVariableSO : ScriptableVariableBaseSO<Material> { }
}