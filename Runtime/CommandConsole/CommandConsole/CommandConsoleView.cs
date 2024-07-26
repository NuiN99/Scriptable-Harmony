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
        [SerializeField] InputActionProperty toggleConsoleInput;
        [SerializeField, HideInInspector] InputAction deleteLastWordInputAction;
        [SerializeField, HideInInspector] InputAction autoCompleteInputAction;
        
        [Header("Dependencies")]
        [SerializeField] CommandConsolePresenter presenter;
        [SerializeField] RectTransform panelRoot;
        [SerializeField] Transform messagesRoot;
        [SerializeField] ScrollRect messagesScrollRect;
        [SerializeField] Transform autoCompleteRoot;
        [SerializeField] TMP_Text autoCompleteOptionPrefab;
        [SerializeField] TMP_InputField textInput;
        [SerializeField] TMP_Text inputPlaceholderText;
        [SerializeField] HoldButton scaleButton;
        [SerializeField] HoldButton moveButton;
        [SerializeField] Button clearButton;
        [SerializeField] Toggle collapseMessagesToggle;
        
#if UNITY_EDITOR
        void OnValidate()
        {
            if (presenter == null) return; 
            presenter.RegisterAssemblies();
        }
#endif
        void OnEnable()
        {
            textInput.onSubmit.AddListener(InvokeCommandHandler);
            textInput.onValueChanged.AddListener(PopulateAutoCompleteOptionsHandler);
            textInput.onSelect.AddListener(PopulateAutoCompleteOptionsOnSelectHandler);
            textInput.onDeselect.AddListener(ClearAutoCompleteOptionsHandler);
            
            moveButton.OnRelease += presenter.ResetInitialPositionValues;
            scaleButton.OnRelease += presenter.ResetInitialSizeValues;
            
            clearButton.onClick.AddListener(ClearMessagesHandler);
            collapseMessagesToggle.onValueChanged.AddListener(CollapseToggleValueChangedHandler);
            
            toggleConsoleInput.action.performed += ToggleConsoleHandler;
            deleteLastWordInputAction.performed += DeleteTextBlockHandler;
            autoCompleteInputAction.performed += FillAutoCompletedTextHandler;

            Application.logMessageReceived += LogMessageRecievedHandler;
        }

        void OnDisable()
        {
            textInput.onSubmit.RemoveListener(InvokeCommandHandler);
            textInput.onValueChanged.RemoveListener(PopulateAutoCompleteOptionsHandler);
            textInput.onSelect.RemoveListener(PopulateAutoCompleteOptionsOnSelectHandler);
            textInput.onDeselect.RemoveListener(ClearAutoCompleteOptionsHandler);
            
            moveButton.OnRelease -= presenter.ResetInitialPositionValues;
            scaleButton.OnRelease -= presenter.ResetInitialSizeValues;
            
            clearButton.onClick.RemoveListener(ClearMessagesHandler);
            collapseMessagesToggle.onValueChanged.RemoveListener(CollapseToggleValueChangedHandler);
            
            toggleConsoleInput.action.performed -= ToggleConsoleHandler;
            deleteLastWordInputAction.performed -= DeleteTextBlockHandler;
            autoCompleteInputAction.performed -= FillAutoCompletedTextHandler;
            
            Application.logMessageReceived -= LogMessageRecievedHandler;
        }
        
        void InvokeCommandHandler(string command)
        {
            presenter.InvokeCommand(textInput);
            presenter.SetScrollRectPosition(messagesScrollRect, 0);
            
            presenter.PopulateAutoCompleteOptions(autoCompleteRoot, autoCompleteOptionPrefab, inputPlaceholderText, textInput.text);
        }

        void ToggleConsoleHandler(InputAction.CallbackContext context) => presenter.ToggleConsole(panelRoot.gameObject);
        void DeleteTextBlockHandler(InputAction.CallbackContext context) => presenter.DeleteTextBlock(textInput);
        void FillAutoCompletedTextHandler(InputAction.CallbackContext context) => presenter.FillAutoCompletedText(textInput);
        void LogMessageRecievedHandler(string message, string stackTrace, LogType logType) => presenter.CreateAndInitializeNewLog(messagesRoot, message, stackTrace, logType);
        void CollapseToggleValueChangedHandler(bool value) =>  presenter.ToggleMessageCollapsing(value);
        void PopulateAutoCompleteOptionsHandler(string text) => presenter.PopulateAutoCompleteOptions(autoCompleteRoot, autoCompleteOptionPrefab, inputPlaceholderText, textInput.text);
        void PopulateAutoCompleteOptionsOnSelectHandler(string text) => presenter.PopulateAutoCompleteOptions(autoCompleteRoot, autoCompleteOptionPrefab, inputPlaceholderText, textInput.text, true);
        void ClearAutoCompleteOptionsHandler(string text) => presenter.ClearAutoCompleteOptions();
        void ClearMessagesHandler() => presenter.ClearMessages(messagesRoot);

        void Awake()
        {
            presenter.RegisterCommands();
            presenter.LoadSavedValues(panelRoot, collapseMessagesToggle);
            
            toggleConsoleInput.action.Enable();
            deleteLastWordInputAction.Enable();
            autoCompleteInputAction.Enable();
            
            panelRoot.gameObject.SetActive(false);
        }

        void Update()
        {
            if (scaleButton.Pressed) presenter.UpdateSize(panelRoot, scaleButton.PressOffset);
            if (moveButton.Pressed) presenter.UpdatePosition(panelRoot);
        }
    }
}
