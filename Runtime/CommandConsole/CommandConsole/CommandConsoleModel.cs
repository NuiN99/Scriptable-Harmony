using System.Collections.Generic;
using System.Reflection;
using NuiN.NExtensions;
using UnityEngine;

namespace NuiN.CommandConsole
{
    public class CommandConsoleModel : MonoBehaviour
    {
        const string POSITION_X_KEY = "COMMAND_CONSOLE_POSITION_X";
        const string POSITION_Y_KEY = "COMMAND_CONSOLE_POSITION_Y";
        
        const string SIZE_X_KEY = "COMMAND_CONSOLE_SIZE_X";
        const string SIZE_Y_KEY = "COMMAND_CONSOLE_SIZE_Y";
        
        [field: SerializeField] public ConsoleAssemblyContainer AssemblyContainer { get; private set; }
        [field: SerializeField] public Vector2 MinSize { get; private set; } = new(200, 200);
        [field: SerializeField] public Vector2 MaxScale { get; private set; } = new(1920, 1080);
        public Dictionary<string, MethodInfo> RegisteredCommands { get; set; } = new();
        
        public Vector2 ConsolePosition { get; set; }
        public Vector2 ConsoleSize { get; set; }
        
        public Vector2 InitialMovePos { get; set; }
        public Vector2 InitialScalePos { get; set; }
        public Vector2 InitialScale { get; set; }

        public Vector2 GetSavedPosition()
        {
            float x = PlayerPrefs.HasKey(POSITION_X_KEY) ? PlayerPrefs.GetFloat(POSITION_X_KEY) : ConsolePosition.x;
            float y = PlayerPrefs.HasKey(POSITION_Y_KEY) ? PlayerPrefs.GetFloat(POSITION_Y_KEY) : ConsolePosition.y;

            float maxX = Screen.width - ConsoleSize.x;
            float maxY = Screen.height - ConsoleSize.y;
            
            x = Mathf.Clamp(x, 0, maxX);
            y = Mathf.Clamp(y, 0, maxY);

            return new Vector2(x, y);
        }
        
        public Vector2 GetSavedSize()
        {
            float x = PlayerPrefs.HasKey(SIZE_X_KEY) ? PlayerPrefs.GetFloat(SIZE_X_KEY) : ConsoleSize.x;
            float y = PlayerPrefs.HasKey(SIZE_Y_KEY) ? PlayerPrefs.GetFloat(SIZE_Y_KEY) : ConsoleSize.y;
            
            x = Mathf.Clamp(x, MinSize.x, MaxScale.x);
            y = Mathf.Clamp(y, MinSize.y, MaxScale.y);

            return new Vector2(x, y);
        }

        public void SetSavedPosition()
        {
            PlayerPrefs.SetFloat(POSITION_X_KEY, ConsolePosition.x);
            PlayerPrefs.SetFloat(POSITION_Y_KEY, ConsolePosition.y);

            PlayerPrefs.Save();
        }
        
        public void SetSavedScale()
        {
            PlayerPrefs.SetFloat(SIZE_X_KEY, ConsoleSize.x);
            PlayerPrefs.SetFloat(SIZE_Y_KEY, ConsoleSize.y);
            
            PlayerPrefs.Save();
        }
    }
}