using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace NuiN.CommandConsole
{
    public class CommandConsoleView : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] InputActionProperty toggleConsoleInput;
        [SerializeField, HideInInspector] InputAction deleteLastWordInputAction;
        
        [Header("Dependencies")]
        [SerializeField] CommandConsolePresenter presenter;
        [SerializeField] RectTransform panelRoot;
        [SerializeField] Transform messagesRoot;
        [SerializeField] TMP_InputField textInput;
        [SerializeField] HoldButton scaleButton;
        [SerializeField] HoldButton moveButton;
        
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
            
            moveButton.OnRelease += presenter.ResetInitialPositionValues;
            scaleButton.OnRelease += presenter.ResetInitialSizeValues;
            
            toggleConsoleInput.action.performed += ToggleConsoleHandler;
            deleteLastWordInputAction.performed += DeleteTextBlockHandler;

            Application.logMessageReceived += LogMessageRecievedHandler;
        }

        void OnDisable()
        {
            textInput.onSubmit.RemoveListener(InvokeCommandHandler);
            
            moveButton.OnRelease -= presenter.ResetInitialPositionValues;
            scaleButton.OnRelease -= presenter.ResetInitialSizeValues;
            
            toggleConsoleInput.action.performed -= ToggleConsoleHandler;
            deleteLastWordInputAction.performed -= DeleteTextBlockHandler;
            
            Application.logMessageReceived -= LogMessageRecievedHandler;
        }
        
        void InvokeCommandHandler(string command) => presenter.InvokeCommand(textInput);
        void ToggleConsoleHandler(InputAction.CallbackContext context) => presenter.ToggleConsole(panelRoot.gameObject);
        void DeleteTextBlockHandler(InputAction.CallbackContext context) => presenter.DeleteTextBlock(textInput);
        void LogMessageRecievedHandler(string message, string stackTrace, LogType logType) => presenter.CreateAndInitializeNewLog(messagesRoot, message, stackTrace, logType);

        void Awake()
        {
            presenter.RegisterCommands();
            presenter.LoadSavedScaleAndPosition(panelRoot);
            
            toggleConsoleInput.action.Enable();
            deleteLastWordInputAction.Enable();
        }

        void Update()
        {
            if (scaleButton.Pressed) presenter.UpdateSize(panelRoot, scaleButton.PressOffset);
            if (moveButton.Pressed) presenter.UpdatePosition(panelRoot);

            if (EventSystem.current.currentSelectedGameObject == textInput.gameObject)
            {
                presenter.AutoCompleteAndSetCommand(textInput.text);
            }
        }
    }
}
