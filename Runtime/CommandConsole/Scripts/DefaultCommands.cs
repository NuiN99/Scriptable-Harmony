using NuiN.NExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NuiN.CommandConsole
{
    public static class DefaultCommands
    {
        [ConsoleCommand("reload-scene")]
        static void ReloadSceneCommand() => GeneralUtils.ReloadScene();
        
        [ConsoleCommand("load-scene")]
        static void LoadSceneCommand(int sceneIndex) => SceneManager.LoadScene(sceneIndex);

        [ConsoleCommand("timescale")]
        static void TimeScaleCommand(float value) => Time.timeScale = value;
        
        [ConsoleCommand("framerate")]
        static void FrameRateCommand(int value) => Application.targetFrameRate = value;
        
        [ConsoleCommand("quit")]
        static void QuitCommand() => Application.Quit();
        
        [ConsoleCommand("log")]
        static void LogCommand(string message) => Debug.Log(message);

        [ConsoleCommand("stupid")]
        static void StupidCommand(string str, int num, float dec) => Debug.Log("you are stupid...");
    }
}