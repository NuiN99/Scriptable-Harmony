using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NuiN.CommandConsole
{
    public class ConsoleMessage : MonoBehaviour
    {
        public MessageKey Key { get; private set; }
        
        [SerializeField] TMP_Text messageText;
        [SerializeField] TMP_Text messageCountText;
        [SerializeField] Image iconImage;
        [SerializeField] ConsoleMessageIconsSO iconsContainer;

        int _messageCount;

        public void Initialize(MessageKey key)
        {
            messageText.SetText(key.message);
            iconImage.sprite = iconsContainer.GetIcon(key.logType);
            IncrementMessageCount();
        }

        public void IncrementMessageCount()
        {
            _messageCount++;
            messageCountText.SetText(_messageCount.ToString());
        }
    }
}