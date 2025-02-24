using NuiN.NExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NuiN.CommandConsole
{
    public static class DefaultCommands
    {
        [ConsoleCommand("reload-scene")]
        public static void ReloadSceneCommand() => GeneralUtils.ReloadScene();
        
        [ConsoleCommand("load-scene")]
        public static void LoadSceneCommand(int sceneIndex) => SceneManager.LoadScene(sceneIndex);

        [ConsoleCommand("timescale")]
        public static void TimeScaleCommand(float value) => Time.timeScale = value;
        
        [ConsoleCommand("framerate")]
        public static void FrameRateCommand(int value) => Application.targetFrameRate = value;
        
        [ConsoleCommand("quit")]
        public static void QuitCommand() => Application.Quit();
        
        [ConsoleCommand("log")]
        public static void LogCommand(string message) => Debug.Log(message);
    }
}