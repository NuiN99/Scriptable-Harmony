using System.Reflection;
using UnityEngine;

namespace NuiN.CommandConsole
{
    public static class DefaultCommands
    {
        [Command("zz")]
        static void Test2(){ }
        
        [Command("add")]
        public static float Add(float num1, float num2) => num1 + num2;

        [Command("print")]
        public static void Print(string message) => Debug.LogError(message);

        [Command("v2")]
        public static Vector2 Vector2(Vector2 v) => v;
        
        [Command("v3")]
        public static Vector3 Vector3(Vector3 v) => v;
        
        [Command("v2int")]
        public static Vector2Int Vector2Int(Vector2Int v) => v;
        
        [Command("v3int")]
        public static Vector3Int Vector3Int(Vector3Int v) => v;
    }
}