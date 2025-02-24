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
        [Header("Input")]
        [SerializeField] InputActionProperty toggleConsoleInputAction;
        [SerializeField] InputActionProperty deleteLastWordInputAction;
        [SerializeField] InputActionProperty autoCompleteInputAction;
        [SerializeField] InputActionProperty commandHistoryUpInputAction;
        [SerializeField] InputActionProperty commandHistoryDownInputAction;
        [SerializeField] InputActionProperty closeConsoleInputAction;
        
        [Header("Dependencies")]
        [SerializeField, InjectComponent] CommandConsolePresenter presenter;
        [SerializeField] RectTransform panelRoot;
        [SerializeField] Transform messagesRoot;
        [SerializeField] ScrollRect messagesScrollRect;
        [SerializeField] TMP_InputField textInput;
        [SerializeField] TMP_Text inputPlaceholderText;
        [SerializeField] Button clearButton;
        [SerializeField] Toggle collapseMessagesToggle;

        void OnEnable()
        {
            textInput.onSubmit.AddListener(InvokeCommandHandler);
            textInput.onValueChanged.AddListener(PopulateAutoCompleteOptionsHandler);
            textInput.onSelect.AddListener(PopulateAutoCompleteOptionsOnSelectHandler);
            
            textInput.onEndEdit.AddListener(InputDeselectedHandler);
            
            clearButton.onClick.AddListener(ClearMessagesHandler);
            collapseMessagesToggle.onValueChanged.AddListener(CollapseToggleValueChangedHandler);
            
            toggleConsoleInputAction.action.performed += ToggleConsoleHandler;
            deleteLastWordInputAction.action.performed += DeleteTextBlockHandler;
            autoCompleteInputAction.action.performed += FillAutoCompletedTextHandler;
            
            Application.logMessageReceived += LogMessageRecievedHandler;
        }

        void OnDisable()
        {
            textInput.onSubmit.RemoveListener(InvokeCommandHandler);
            textInput.onValueChanged.RemoveListener(PopulateAutoCompleteOptionsHandler);
            textInput.onSelect.RemoveListener(PopulateAutoCompleteOptionsOnSelectHandler);
            
            textInput.onEndEdit.RemoveListener(InputDeselectedHandler);
            
            clearButton.onClick.RemoveListener(ClearMessagesHandler);
            collapseMessagesToggle.onValueChanged.RemoveListener(CollapseToggleValueChangedHandler);
            
            toggleConsoleInputAction.action.performed -= ToggleConsoleHandler;
            deleteLastWordInputAction.action.performed -= DeleteTextBlockHandler;
            autoCompleteInputAction.action.performed -= FillAutoCompletedTextHandler;
            
            Application.logMessageReceived -= LogMessageRecievedHandler;
        }
        
        void InvokeCommandHandler(string command) => presenter.SubmitCommand(textInput, inputPlaceholderText, messagesScrollRect, panelRoot);
        void ToggleConsoleHandler(InputAction.CallbackContext context) => presenter.ToggleConsole(panelRoot.gameObject, textInput);
        void DeleteTextBlockHandler(InputAction.CallbackContext context) => presenter.DeleteTextBlock(textInput);
        void FillAutoCompletedTextHandler(InputAction.CallbackContext context) => presenter.FillAutoCompletedText(textInput);
        void LogMessageRecievedHandler(string message, string stackTrace, LogType logType) => presenter.CreateAndInitializeNewLog(messagesRoot, message, stackTrace, logType);
        void CollapseToggleValueChangedHandler(bool value) =>  presenter.ToggleMessageCollapsing(value);
        void PopulateAutoCompleteOptionsHandler(string text) => presenter.UpdatePlaceholderText(inputPlaceholderText, textInput);
        void PopulateAutoCompleteOptionsOnSelectHandler(string text) => presenter.UpdatePlaceholderText(inputPlaceholderText, textInput, true);
        void InputDeselectedHandler(string text)
        {
            if (!Input.GetKey(KeyCode.Escape)) return;
            presenter.DisableConsole(panelRoot.gameObject, textInput);
        }

        void ClearMessagesHandler() => presenter.ClearMessages(messagesRoot);

        void Awake()
        {
            presenter.RegisterCommands();
            presenter.LoadSavedValues(panelRoot, collapseMessagesToggle);
            
            toggleConsoleInputAction.action.Enable();
            deleteLastWordInputAction.action.Enable();
            autoCompleteInputAction.action.Enable();
            
            panelRoot.gameObject.SetActive(false);
        }
    }
}
