using System.Collections.Generic;
using System.Reflection;
using NuiN.NExtensions;
using UnityEngine;

namespace NuiN.CommandConsole
{
    public class CommandConsoleModel : MonoBehaviour
    {
        public Dictionary<string, MethodInfo> RegisteredCommands { get; set; } = new();
        [field: SerializeField, ReadOnly] public List<string> RegisteredAssemblies { get; set; } = new();
    
        public Vector2 InitialMovePos { get; set; }
        public Vector2 InitialScalePos { get; set; }
        public Vector2 InitialScale { get; set; }

        [field: SerializeField] public Vector2 MinScale { get; private set; } = new(200, 200);
        [field: SerializeField] public Vector2 MaxScale { get; private set; } = new(1920, 1080);
    }
}