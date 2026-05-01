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
        [SerializeField, Min(0f)] float autocompleteRepeatDelay = 0.35f;
        [SerializeField, Min(0.01f)] float autocompleteRepeatRate = 0.08f;
        
        [Header("Dependencies")]
        [SerializeField, InjectComponent] CommandConsolePresenter presenter;
        [SerializeField] RectTransform panelRoot;
        [SerializeField] Transform messagesRoot;
        [SerializeField] ScrollRect messagesScrollRect;
        [SerializeField] TMP_InputField textInput;
        [SerializeField] TMP_Text inputPlaceholderText;
        [SerializeField] TMP_Text autocompleteOptionsText;
        [SerializeField] Button closeButton;
        [SerializeField] Button clearButton;
        [SerializeField] Toggle collapseMessagesToggle;
        
        float autocompleteRepeatTimer;
        int autocompleteRepeatDirection;

        void OnEnable()
        {
            textInput.onSubmit.AddListener(InvokeCommandHandler);
            textInput.onValueChanged.AddListener(PopulateAutoCompleteOptionsHandler);
            textInput.onSelect.AddListener(PopulateAutoCompleteOptionsOnSelectHandler);
            
            textInput.onEndEdit.AddListener(InputDeselectedHandler);
            
            clearButton.onClick.AddListener(ClearMessagesHandler);
            closeButton.onClick.AddListener(ToggleConsoleHandler);
            collapseMessagesToggle.onValueChanged.AddListener(CollapseToggleValueChangedHandler);
            
            toggleConsoleInputAction.action.performed += ToggleConsoleInputHandler;
            deleteLastWordInputAction.action.performed += DeleteTextBlockHandler;
            autoCompleteInputAction.action.performed += FillAutoCompletedTextHandler;
            commandHistoryUpInputAction.action.performed += CycleAutocompleteUpHandler;
            commandHistoryDownInputAction.action.performed += CycleAutocompleteDownHandler;
            
            presenter.OnCommandLogRecieved += MessageRecievedHandler;
        }

        void OnDisable()
        {
            textInput.onSubmit.RemoveListener(InvokeCommandHandler);
            textInput.onValueChanged.RemoveListener(PopulateAutoCompleteOptionsHandler);
            textInput.onSelect.RemoveListener(PopulateAutoCompleteOptionsOnSelectHandler);
            
            textInput.onEndEdit.RemoveListener(InputDeselectedHandler);
            
            closeButton.onClick.RemoveListener(ToggleConsoleHandler);
            clearButton.onClick.RemoveListener(ClearMessagesHandler);
            collapseMessagesToggle.onValueChanged.RemoveListener(CollapseToggleValueChangedHandler);
            
            toggleConsoleInputAction.action.performed -= ToggleConsoleInputHandler;
            deleteLastWordInputAction.action.performed -= DeleteTextBlockHandler;
            autoCompleteInputAction.action.performed -= FillAutoCompletedTextHandler;
            commandHistoryUpInputAction.action.performed -= CycleAutocompleteUpHandler;
            commandHistoryDownInputAction.action.performed -= CycleAutocompleteDownHandler;
            
            presenter.OnCommandLogRecieved -= MessageRecievedHandler;
        }
        
        void InvokeCommandHandler(string command) => presenter.SubmitCommand(textInput, inputPlaceholderText, autocompleteOptionsText, messagesScrollRect, panelRoot);
        void ToggleConsoleInputHandler(InputAction.CallbackContext context) => ToggleConsole();
        void ToggleConsoleHandler() => ToggleConsole();
        void DeleteTextBlockHandler(InputAction.CallbackContext context) => presenter.DeleteTextBlock(textInput, inputPlaceholderText, autocompleteOptionsText);
        void FillAutoCompletedTextHandler(InputAction.CallbackContext context) => presenter.FillAutoCompletedText(textInput);
        void CycleAutocompleteUpHandler(InputAction.CallbackContext context) => presenter.CycleAutocompleteSelection(inputPlaceholderText, autocompleteOptionsText, textInput, -1);
        void CycleAutocompleteDownHandler(InputAction.CallbackContext context) => presenter.CycleAutocompleteSelection(inputPlaceholderText, autocompleteOptionsText, textInput, 1);
        void MessageRecievedHandler(object message, LogType logType) => presenter.CreateAndInitializeNewLog(messagesRoot, message.ToString(), logType);
        void CollapseToggleValueChangedHandler(bool value) =>  presenter.ToggleMessageCollapsing(value);
        void PopulateAutoCompleteOptionsHandler(string text) => presenter.UpdatePlaceholderText(inputPlaceholderText, autocompleteOptionsText, textInput);
        void PopulateAutoCompleteOptionsOnSelectHandler(string text) => presenter.UpdatePlaceholderText(inputPlaceholderText, autocompleteOptionsText, textInput, true);
        void InputDeselectedHandler(string text)
        {
            if (!Input.GetKey(KeyCode.Escape)) return;
            presenter.DisableConsole(panelRoot.gameObject, textInput);
        }

        void ClearMessagesHandler() => presenter.ClearMessages(messagesRoot);
        
        void ToggleConsole()
        {
            presenter.ToggleConsole(panelRoot.gameObject, textInput);
            if (!panelRoot.gameObject.activeSelf) return;
            
            presenter.UpdatePlaceholderText(inputPlaceholderText, autocompleteOptionsText, textInput, true);
        }
        
        void Update()
        {
            int direction = GetHeldAutocompleteDirection();
            if (direction == 0)
            {
                autocompleteRepeatDirection = 0;
                autocompleteRepeatTimer = 0f;
                return;
            }
            
            if (direction != autocompleteRepeatDirection)
            {
                autocompleteRepeatDirection = direction;
                autocompleteRepeatTimer = autocompleteRepeatDelay;
                return;
            }

            autocompleteRepeatTimer -= Time.unscaledDeltaTime;
            if (autocompleteRepeatTimer > 0f) return;
            
            presenter.CycleAutocompleteSelection(inputPlaceholderText, autocompleteOptionsText, textInput, direction);
            autocompleteRepeatTimer = autocompleteRepeatRate;
        }
        
        int GetHeldAutocompleteDirection()
        {
            if (commandHistoryUpInputAction.action.IsPressed()) return -1;
            if (commandHistoryDownInputAction.action.IsPressed()) return 1;
            return 0;
        }

        void Awake()
        {
            InitializeAutocompleteOptionsText();
            
            presenter.RegisterCommands();
            presenter.LoadSavedValues(panelRoot, collapseMessagesToggle);
            
            toggleConsoleInputAction.action.Enable();
            deleteLastWordInputAction.action.Enable();
            autoCompleteInputAction.action.Enable();
            commandHistoryUpInputAction.action.Enable();
            commandHistoryDownInputAction.action.Enable();
            
            panelRoot.gameObject.SetActive(false);
        }
        
        void InitializeAutocompleteOptionsText()
        {
            const float maxAutocompletePanelHeight = 180f;

            GameObject autocompleteOptionsPanel = new GameObject("AutocompleteOptionsPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(RectMask2D), typeof(LayoutElement));
            autocompleteOptionsPanel.transform.SetParent(textInput.transform, false);
            autocompleteOptionsPanel.transform.SetAsLastSibling();

            RectTransform panelRect = autocompleteOptionsPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0f, 1f);
            panelRect.anchorMax = new Vector2(1f, 1f);
            panelRect.pivot = new Vector2(0.5f, 0f);
            panelRect.anchoredPosition = Vector2.zero;
            panelRect.sizeDelta = new Vector2(0f, 0f);

            Image panelImage = autocompleteOptionsPanel.GetComponent<Image>();
            panelImage.color = new Color(0f, 0f, 0f, 0.78f);
            panelImage.raycastTarget = false;

            LayoutElement panelLayoutElement = autocompleteOptionsPanel.GetComponent<LayoutElement>();
            panelLayoutElement.ignoreLayout = true;
            panelLayoutElement.preferredHeight = maxAutocompletePanelHeight;
            panelLayoutElement.flexibleHeight = 0;

            GameObject autocompleteOptionsObject = new GameObject("AutocompleteOptions", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            autocompleteOptionsObject.transform.SetParent(autocompleteOptionsPanel.transform, false);

            RectTransform optionsRect = autocompleteOptionsObject.GetComponent<RectTransform>();
            optionsRect.anchorMin = new Vector2(0f, 1f);
            optionsRect.anchorMax = new Vector2(1f, 1f);
            optionsRect.pivot = new Vector2(0.5f, 1f);
            optionsRect.anchoredPosition = new Vector2(8f, -4f);
            optionsRect.sizeDelta = new Vector2(-16f, 0f);

            autocompleteOptionsText = autocompleteOptionsObject.GetComponent<TextMeshProUGUI>();
            autocompleteOptionsText.font = inputPlaceholderText.font;
            autocompleteOptionsText.fontSharedMaterial = inputPlaceholderText.fontSharedMaterial;
            autocompleteOptionsText.fontSize = inputPlaceholderText.fontSize;
            autocompleteOptionsText.color = new Color(1f, 1f, 1f, 0.65f);
            autocompleteOptionsText.textWrappingMode = TextWrappingModes.NoWrap;
            autocompleteOptionsText.overflowMode = TextOverflowModes.Overflow;
            autocompleteOptionsText.richText = true;
            autocompleteOptionsText.alignment = TextAlignmentOptions.Left;
            autocompleteOptionsText.raycastTarget = false;
            autocompleteOptionsText.SetText(string.Empty);
        }
    }
}
