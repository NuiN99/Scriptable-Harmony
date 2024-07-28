#if UNITY_EDITOR
using UnityEditor;

namespace NuiN.NExtensions
{
    // modified from https://github.com/BrainswitchMachina/Show-In-Inspector
    [CustomEditor( typeof( UnityEngine.MonoBehaviour ), true )]
    public class ShowInInspectorMonoBehaviourEditor : ShowInInspectorEditor { }
}
#endif