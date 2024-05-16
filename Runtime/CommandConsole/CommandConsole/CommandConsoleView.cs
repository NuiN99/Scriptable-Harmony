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
        
        void OnEnable()
        {
            textInput.onSubmit.AddListener(InvokeCommandHandler);
            moveButton.OnRelease += presenter.ResetInitialPositionValues;
            scaleButton.OnRelease += presenter.ResetInitialSizeValues;
        }

        void OnDisable()
        {
            textInput.onSubmit.RemoveListener(InvokeCommandHandler);
            moveButton.OnRelease -= presenter.ResetInitialPositionValues;
            scaleButton.OnRelease -= presenter.ResetInitialSizeValues;
        }
    
        void Awake() => presenter.RegisterCommands();
        void Start() => presenter.LoadSavedScaleAndPosition(panelRoot);
        void InvokeCommandHandler(string command) => presenter.InvokeCommand(command, textInput);
        void Update() => MoveAndScalePanel();
    
        void MoveAndScalePanel()
        {
            if (scaleButton.Pressed) presenter.UpdateSize(panelRoot, scaleButton.PressOffset);
            if (moveButton.Pressed) presenter.UpdatePosition(panelRoot);
        }
    }
}
