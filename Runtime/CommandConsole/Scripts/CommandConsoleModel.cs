using System.Collections.Generic;
using System.Reflection;
using NuiN.NExtensions;
using TMPro;
using UnityEngine;

namespace NuiN.CommandConsole
{
    public class CommandConsoleModel : MonoBehaviour
    {
        const string COLLAPSE_MESSAGES_KEY = "COMMAND_CONSOLE_COLLAPSE_MESSAGES";
        
        [field: SerializeField] public ConsoleMessage ConsoleMessagePrefab { get; private set; }
        public Dictionary<CommandKey, MethodInfo> RegisteredCommands { get; set; } = new();
        public Dictionary<MessageKey, ConsoleMessage> Logs { get; set; } = new();
        public CommandKey SelectedCommand { get; set; }
        public bool IsConsoleEnabled { get; set; }
        public bool CollapseMessages { get; set; }

        public bool GetSavedCollapseMessagesValue()
        {
            bool collapseMessages = GeneralUtils.GetPrefsBool(CollapseMessages, COLLAPSE_MESSAGES_KEY);
            CollapseMessages = collapseMessages;
            return collapseMessages;
        }

        public void SetSavedCollapseMessagesValue() => GeneralUtils.SetPrefsBool(CollapseMessages, COLLAPSE_MESSAGES_KEY);
    }
}