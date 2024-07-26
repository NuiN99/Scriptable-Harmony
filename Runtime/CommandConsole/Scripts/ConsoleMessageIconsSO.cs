using UnityEngine;

namespace NuiN.CommandConsole
{
    public class ConsoleMessageIconsSO : ScriptableObject
    {
        [SerializeField] Sprite logIcon;
        [SerializeField] Sprite warningIcon;
        [SerializeField] Sprite errorIcon;
        [SerializeField] Sprite exceptionIcon;
        [SerializeField] Sprite assertIcon;

        public Sprite GetIcon(LogType logType)
        {
            return logType switch
            {
                LogType.Log => logIcon,
                LogType.Warning => warningIcon,
                LogType.Error => errorIcon,
                LogType.Exception => exceptionIcon,
                LogType.Assert => assertIcon,
                _ => logIcon
            };
        }
    }
}