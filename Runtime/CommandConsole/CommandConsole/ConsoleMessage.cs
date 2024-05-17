using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NuiN.CommandConsole
{
    public class ConsoleMessage : MonoBehaviour
    {
        [SerializeField] TMP_Text messageText;
        [SerializeField] Image iconImage;

        [SerializeField] ConsoleMessageIconsSO iconsContainer;

        public void Initialize(string str, LogType logType)
        {
            messageText.SetText(str);
            iconImage.sprite = iconsContainer.GetIcon(logType);
        }
    }
}