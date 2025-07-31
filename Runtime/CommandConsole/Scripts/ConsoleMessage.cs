using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NuiN.CommandConsole
{
    public class ConsoleMessage : MonoBehaviour
    {
        [SerializeField] TMP_Text messageText;

        public void Initialize(MessageKey key, Color color)
        {
            messageText.SetText($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{key.message}</color>");
        }
    }
}