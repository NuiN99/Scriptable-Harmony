using UnityEngine;
using NuiN.ScriptableHarmony.ListVariable.Base;

namespace NuiN.ScriptableHarmony.ListVariable.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/ListVariables/AudioClip",
        fileName = "New AudioClip ListVariable")]
    internal class AudioClipListVariableSO : ScriptableListVariableBaseSO<AudioClip> { }
}