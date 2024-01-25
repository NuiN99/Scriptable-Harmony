using NuiN.ScriptableHarmony.Core;
using UnityEngine;

namespace NuiN.ScriptableHarmony
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/Variables/string", 
        fileName = "New String Variable")]
    internal class StringSO : ScriptableVariableSO<string> { }
}