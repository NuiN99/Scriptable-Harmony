using System.Collections.Generic;
using System.Reflection;
using NuiN.NExtensions;
using TMPro;
using UnityEngine;

namespace NuiN.CommandConsole
{
    public class CommandConsoleModel : MonoBehaviour
    {
        [field: SerializeField] public ConsoleMessage ConsoleMessagePrefab { get; private set; }
        public Dictionary<CommandKey, MethodInfo> RegisteredCommands { get; set; } = new();
        public List<ConsoleMessage> Logs { get; set; } = new();
        public CommandKey SelectedCommand { get; set; }
        public bool IsConsoleEnabled { get; set; }
    }
}