using NuiN.ScriptableHarmony.Core;
using UnityEngine;

namespace NuiN.ScriptableHarmony
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/ListVariables/AudioClip",
        fileName = "New AudioClip ListVariable")]
    internal class AudioClipListSO : ScriptableListSO<AudioClip> { }
}