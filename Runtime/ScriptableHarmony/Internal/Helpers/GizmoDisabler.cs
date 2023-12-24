using System;
using System.Reflection;

namespace NuiN.ScriptableHarmony.Internal.Helpers
{
    internal static class GizmoDisabler
    {
#if UNITY_EDITOR
        static MethodInfo _setIconEnabled;
        static MethodInfo SetIconEnabled => 
            _setIconEnabled ??= Assembly.GetAssembly( typeof(UnityEditor.Editor) )
                ?.GetType( "UnityEditor.AnnotationUtility" )
                ?.GetMethod( "SetIconEnabled", BindingFlags.Static | BindingFlags.NonPublic );
 #endif
        
        public static void SetGizmoIconEnabled( Type type, bool on ) {
#if UNITY_EDITOR
            if( SetIconEnabled == null ) return;
            const int monoBehaviorClassID = 114; // https://docs.unity3d.com/Manual/ClassIDReference.html
            SetIconEnabled.Invoke( null, new object[] { monoBehaviorClassID, type.Name, on ? 1 : 0 } );
#endif
        }
    } 
}

