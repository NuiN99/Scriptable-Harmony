using NuiN.ScriptableHarmony.Variable.Base;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Variable.Common
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/Variables/AudioClip", 
        fileName = "New AudioClip Variable")]
    internal class AudioClipSO : ScriptableVariableSO<AudioClip> { }
}