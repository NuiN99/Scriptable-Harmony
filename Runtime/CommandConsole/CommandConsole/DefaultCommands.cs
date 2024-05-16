using UnityEngine;

namespace NuiN.CommandConsole
{
    public static class DefaultCommands
    {
        [Command("add")]
        public static float Add(float num1, float num2) => num1 + num2;

        [Command("print")]
        public static void Print(string message) => Debug.LogError(message);
    }
}