using System;

namespace NuiN.CommandConsole
{
    public static class CommandConsoleEvents
    {
        public static event Action OnOpen;
        public static event Action OnClose;

        internal static void InvokeOpen() => OnOpen?.Invoke();
        internal static void InvokeClose() => OnClose?.Invoke();
    }
}