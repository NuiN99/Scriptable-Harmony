using UnityEngine;
using NuiN.ScriptableHarmony.Core;

namespace NuiN.ScriptableHarmony
{   
    [CreateAssetMenu(
        menuName = "ScriptableHarmony/Common/Dictionaries/String/Int",
        fileName = "New String/Int Dictionary")]
    internal class StringToIntDictionarySO : ScriptableDictionarySO<string, int> { }
}