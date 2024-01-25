using UnityEngine;
using NuiN.ScriptableHarmony.ListVariable.Base;

namespace NuiN.ScriptableHarmony.ListVariable.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/ListVariables/Sprite",
        fileName = "New Sprite ListVariable")]
    internal class SpriteListSO : ScriptableListSO<Sprite> { }
}