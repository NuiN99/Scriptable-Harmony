using NuiN.NExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NuiN.CommandConsole
{
    public static class DefaultCommands
    {
        [ConsoleCommand("reload-scene", header: "Default")]
        static void ReloadSceneCommand() => GeneralUtils.ReloadScene();
        
        [ConsoleCommand("load-scene", header: "Default")]
        static void LoadSceneCommand(int sceneIndex) => SceneManager.LoadScene(sceneIndex);

        [ConsoleCommand("timescale", header: "Default")]
        static void TimeScaleCommand(float value) => Time.timeScale = value;
        
        [ConsoleCommand("framerate", header: "Default")]
        static void FrameRateCommand(int value) => Application.targetFrameRate = value;
        
        [ConsoleCommand("quit", header: "Default")]
        static void QuitCommand() => Application.Quit();
        
        [ConsoleCommand("log", header: "Logging")]
        static void LogCommand(string message) => Debug.Log(message);
        
        [ConsoleCommand("error", header: "Logging")]
        static void LogErrorCommand(string message) => Debug.LogError(message);
        
        [ConsoleCommand("warn", header: "Logging")]
        static void LogWarningCommand(string message) => Debug.LogWarning(message);
    }
}