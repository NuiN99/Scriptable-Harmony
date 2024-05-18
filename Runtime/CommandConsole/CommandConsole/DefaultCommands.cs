using System.Collections;
using UnityEngine;

namespace NuiN.CommandConsole
{
    public static class DefaultCommands
    {
        [Command("zz")]
        static void Test2(){ }
        
        [Command("add")]
        static float Add(float num1, float num2) => num1 + num2;

        [Command("error")]
        static void Error(string message) => Debug.LogError(message);

        [Command("v2")]
        static Vector2 Vector2(Vector2 v) => v;
        
        [Command("v3")]
        static Vector3 Vector3(Vector3 v) => v;
        
        [Command("v2int")]
        static Vector2Int Vector2Int(Vector2Int v) => v;
        
        [Command("v3int")]
        static Vector3Int Vector3Int(Vector3Int v) => v;

        [Command("coroutine")]
        static IEnumerator Coroutine(string message)
        {
            yield return new WaitForSeconds(1);
            Debug.Log(message);
        }
    }
}