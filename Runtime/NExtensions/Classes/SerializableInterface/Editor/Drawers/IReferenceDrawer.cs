using UnityEngine;

namespace NuiN.NExtensions.Editor
{
    internal interface IReferenceDrawer
    {
        float GetHeight();
        void OnGUI(Rect position);
    }
}
