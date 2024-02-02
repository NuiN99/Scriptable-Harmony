using NuiN.ScriptableHarmony.Core;
using UnityEngine;

namespace NuiN.ScriptableHarmony
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/Variables/AudioClip", 
        fileName = "New AudioClip Variable")]
    internal class AudioClipVariableSO : ScriptableVariableSO<AudioClip> { }
}