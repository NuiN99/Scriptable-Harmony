using System;
using NuiN.NExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Toggle = UnityEngine.UI.Toggle;

namespace NuiN.CommandConsole
{
    public class CommandConsoleView : MonoBehaviour
    {
        public bool IsOpen => presenter.IsOpen;
        
        [Header("Input")]
        [SerializeField] InputActionProperty deleteLastWordInputAction;
        [SerializeField] InputActionProperty autoCompleteInputAction;
        [SerializeField] InputActionProperty commandHistoryUpInputAction;
        [SerializeField] InputActionProperty commandHistoryDownInputAction;
        
        [Header("Dependencies")]
        [SerializeField, InjectComponent] CommandConsolePresenter presenter;
        [SerializeField] RectTransform panelRoot;
        [SerializeField] Transform messagesRoot;
        [SerializeField] ScrollRect messagesScrollRect;
        [SerializeField] TMP_InputField textInput;
        [SerializeField] TMP_Text inputPlaceholderText;

        [Header("Visuals")] 
        [SerializeField] Color logColor;
        [SerializeField] Color warnColor;
        [SerializeField] Color errorColor;

        void OnEnable()
        {
            textInput.onSubmit.AddListener(InvokeCommandHandler);
            textInput.onValueChanged.AddListener(PopulateAutoCompleteOptionsHandler);
            textInput.onSelect.AddListener(PopulateAutoCompleteOptionsOnSelectHandler);
            
            textInput.onEndEdit.AddListener(InputDeselectedHandler);
            
            deleteLastWordInputAction.action.performed += DeleteTextBlockHandler;
            autoCompleteInputAction.action.performed += FillAutoCompletedTextHandler;

            presenter.OnCreateLog += OnCreateLog;
        }

        void OnDisable()
        {
            textInput.onSubmit.RemoveListener(InvokeCommandHandler);
            textInput.onValueChanged.RemoveListener(PopulateAutoCompleteOptionsHandler);
            textInput.onSelect.RemoveListener(PopulateAutoCompleteOptionsOnSelectHandler);
            
            textInput.onEndEdit.RemoveListener(InputDeselectedHandler);
            
            deleteLastWordInputAction.action.performed -= DeleteTextBlockHandler;
            autoCompleteInputAction.action.performed -= FillAutoCompletedTextHandler;
            
            presenter.OnCreateLog -= OnCreateLog;
        }
        
        void InvokeCommandHandler(string command) => presenter.SubmitCommand(textInput, inputPlaceholderText, messagesScrollRect, panelRoot);
        void DeleteTextBlockHandler(InputAction.CallbackContext context) => presenter.DeleteTextBlock(textInput);
        void FillAutoCompletedTextHandler(InputAction.CallbackContext context) => presenter.FillAutoCompletedText(textInput);
        void PopulateAutoCompleteOptionsHandler(string text) => presenter.UpdatePlaceholderText(inputPlaceholderText, textInput);
        void PopulateAutoCompleteOptionsOnSelectHandler(string text) => presenter.UpdatePlaceholderText(inputPlaceholderText, textInput, true);

        void OnCreateLog(string message, LogType logType, CommandConsoleModel model)
        {
            MessageKey key = new MessageKey(message, logType);
            ConsoleMessage consoleMessage = Instantiate(model.ConsoleMessagePrefab, messagesRoot);

            Color color = logType switch
            {
                LogType.Log => logColor,
                LogType.Warning => warnColor,
                LogType.Error => errorColor,
                
                _ => logColor
            };
            
            consoleMessage.Initialize(key, color);

            model.Logs.Add(consoleMessage);
        }
        
        void InputDeselectedHandler(string text)
        {
            if (!Input.GetKey(KeyCode.Escape)) return;
            presenter.DisableConsole(panelRoot.gameObject, textInput);
        }

        public void SetConsoleActive(bool isEnabled)
        {
            if (isEnabled) presenter.EnableConsole(panelRoot.gameObject, textInput);
            else presenter.DisableConsole(panelRoot.gameObject, textInput);
        }

        void Awake()
        {
            presenter.RegisterCommands();
            
            deleteLastWordInputAction.action.Enable();
            autoCompleteInputAction.action.Enable();
            
            panelRoot.gameObject.SetActive(false);
        }
    }
}
