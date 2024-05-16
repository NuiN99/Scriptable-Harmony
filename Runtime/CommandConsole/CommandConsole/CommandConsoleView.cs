using NuiN.CommandConsole;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace NuiN.CommandConsole
{
    public class CommandConsoleView : MonoBehaviour
    {
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
    
        void Awake() => presenter.RegisterCommands();
        void OnEnable() => textInput.onSubmit.AddListener(InvokeCommandHandler);
        void OnDisable() => textInput.onSubmit.RemoveListener(InvokeCommandHandler);
        void InvokeCommandHandler(string command) => presenter.InvokeCommand(command, textInput);
        void Update() => MoveAndScalePanel();
    
        void MoveAndScalePanel()
        {
            if (scaleButton.Pressed) presenter.UpdateSize(panelRoot, scaleButton.PressOffset);
            else presenter.ResetInitialSizeValues();

            if (moveButton.Pressed) presenter.UpdatePosition(panelRoot);
            else presenter.ResetInitialPositionValues();
        }
    }
}
