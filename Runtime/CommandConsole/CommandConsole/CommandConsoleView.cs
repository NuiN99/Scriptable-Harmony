using NuiN.CommandConsole;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NuiN.CommandConsole
{
    public class CommandConsoleView : MonoBehaviour
    {
        [SerializeField] InputActionProperty toggleConsoleInput;
        
        [Header("Dependencies")]
        [SerializeField] CommandConsolePresenter presenter;
        [SerializeField] RectTransform panelRoot;
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
        }

        void OnDisable()
        {
            textInput.onSubmit.RemoveListener(InvokeCommandHandler);
            moveButton.OnRelease -= presenter.ResetInitialPositionValues;
            scaleButton.OnRelease -= presenter.ResetInitialSizeValues;
            toggleConsoleInput.action.performed -= ToggleConsoleHandler;
        }
        
        void InvokeCommandHandler(string command) => presenter.InvokeCommand(command, textInput);
        void ToggleConsoleHandler(InputAction.CallbackContext context) => presenter.ToggleConsole(panelRoot.gameObject);
    
        void Awake()
        {
            presenter.RegisterCommands();
            presenter.LoadSavedScaleAndPosition(panelRoot);
            
            toggleConsoleInput.action.Enable();
        }

        void Update()
        {
            if (scaleButton.Pressed) presenter.UpdateSize(panelRoot, scaleButton.PressOffset);
            if (moveButton.Pressed) presenter.UpdatePosition(panelRoot);
        }
    }
}
