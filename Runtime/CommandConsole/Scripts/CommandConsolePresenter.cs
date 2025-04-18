﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NuiN.NExtensions;
using NuiN.ScriptableHarmony;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace NuiN.CommandConsole
{
    public class CommandConsolePresenter : MonoBehaviour
    {
        const string INVALID_PARAMETER = "invalid!";

        [SerializeField, InjectComponent] CommandConsoleModel model;

        public void LoadSavedValues(RectTransform root, Toggle collapseMessagesToggle)
        {
            collapseMessagesToggle.isOn = model.GetSavedCollapseMessagesValue();
        }
        
        public void RegisterCommands()
        {
            model.RegisteredCommands = new Dictionary<CommandKey, MethodInfo>();

            List<Assembly> loadedAssemblies = AssemblyContainerSO.Instance.RegisteredAssemblies.Select(Assembly.Load).ToList();
                
            foreach (var assembly in loadedAssemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                    {
                        var commandAttributes = method.GetCustomAttributes(typeof(ConsoleCommandAttribute), true);
                        foreach (ConsoleCommandAttribute attribute in commandAttributes)
                        {
                            if (!method.IsStatic && !typeof(MonoBehaviour).IsAssignableFrom(method.DeclaringType)) continue;

                            CommandKey commandKey = new CommandKey(attribute.command, method.GetParameters());
                            if (!model.RegisteredCommands.TryAdd(commandKey, method))
                            {
                                Debug.LogWarning($"Command already declared for [{attribute.command}] in [{method.DeclaringType}]");
                            }
                        }
                    }
                }
            }
            
            IOrderedEnumerable<KeyValuePair<CommandKey, MethodInfo>> sortedCommands = 
                from entry in model.RegisteredCommands 
                orderby entry.Key.name descending 
                select entry;
            
            model.RegisteredCommands = new Dictionary<CommandKey, MethodInfo>(sortedCommands);
        }
        
        public void InvokeCommand(TMP_InputField inputField)
        {
            string fullCommand = inputField.text;
            
            // reselect the input field and move the caret to the end for good UX
            inputField.ActivateInputField();
            inputField.caretPosition = fullCommand.Length;
            inputField.SetTextWithoutNotify(string.Empty); 
            
            // no input was detected
            if (fullCommand.Trim().Length <= 0) return;
            
            // the first space after the method name indicates that parameters have been entered
            string[] commandParts = fullCommand.Split(new[] { ' ' }, 2);
            
            if (!model.RegisteredCommands.TryGetValue(model.SelectedCommand, out MethodInfo method))
            {
                Debug.LogWarning("Command not found!");
                return;
            }
            
            ParameterInfo[] parameterInfos = method.GetParameters();
            
            List<ParameterInfo> optionalParams = parameterInfos.Where(param => param.IsOptional).ToList();
            int minParamCount = parameterInfos.Length - optionalParams.Count;
            int maxParamCount = parameterInfos.Length;

            List<object> parameters = new();
            
            // if there is no second section of the full command string, there are no parameters
            bool hasParameters = commandParts.Length > 1;
            if(hasParameters)
            {
                // separate the parameters into their own strings and remove any spaces
                string[] stringParameters = commandParts[1].Split(" ").Where(str => !string.IsNullOrEmpty(str)).ToArray();
                
                if (!HasValidParameterCount(stringParameters.Length, minParamCount, maxParamCount))
                {
                    return;
                }

                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    ParameterInfo parameterInfo = parameterInfos[i];

                    // the optional parameter was not enterered so set it to the default
                    if (i >= stringParameters.Length)
                    {
                        parameters.Add(parameterInfo.DefaultValue);
                        continue;
                    }
                    
                    string stringParam = stringParameters[i];
                    
                    object param = GetParsedArg(parameterInfo.ParameterType, stringParam);
                    
                    if (param == null)
                    {
                        Debug.LogWarning("Invalid Parameter");
                        return;
                    }
                    
                    parameters.Add(param);
                }
            }
            
            if (!HasValidParameterCount(parameters.Count, minParamCount, maxParamCount))
            {
                return;
            }
            
            // if no parameters were entered, attempt to add any default values to the params
            if (!hasParameters || parameters.Count <= 0)
            {
                optionalParams.ForEach(param => parameters.Add(param.DefaultValue));
            }

            try
            {
                if (method.IsStatic)
                {
                    InvokeMethod(method, null, parameters);
                }
                else
                {
                    // find and invoke all instances of the method's class in the scene
                    List<MonoBehaviour> instances = FindObjectsByType(method.DeclaringType, FindObjectsSortMode.None).Select(obj => obj as MonoBehaviour).ToList();

                    if (instances.Count <= 0)
                    {
                        Debug.LogWarning("No instances found to run the command");
                    }

                    foreach (MonoBehaviour instance in instances)
                    {
                        InvokeMethod(method, instance, parameters);
                    }
                }
            }
            catch (Exception err)
            {
                Debug.LogError(err.Message);
            }
        }

        void InvokeMethod(MethodInfo method, MonoBehaviour instance, List<object> parameters)
        {
            object returnValue = method.Invoke(instance, parameters.ToArray());
            if (returnValue == null) return;
            
            if (method.ReturnType == typeof(IEnumerator))
            {
                MonoBehaviour coroutineStarter = instance != null ? instance : this;
                coroutineStarter.StartCoroutine(returnValue as IEnumerator);
                
                return;
            }
            Debug.Log(returnValue);
        }
        
        bool HasValidParameterCount(int inputCount, int minCount, int maxCount)
        {
            if (inputCount < minCount)
            {
                Debug.LogWarning("Not enough parameters!");
                return false;
            }
            if (inputCount > maxCount)
            {
                Debug.LogWarning("Too many parameters!");
                return false;
            }

            return true;
        }

        public void ToggleConsole(GameObject console, TMP_InputField inputField)
        {
            bool isEnabled = !model.IsConsoleEnabled;

            if (isEnabled) EnableConsole(console, inputField);
            else DisableConsole(console, inputField);
        }

        public void EnableConsole(GameObject console, TMP_InputField inputField)
        {
            console.SetActive(true);
            model.IsConsoleEnabled = true;
            
            inputField.ActivateInputField();
            inputField.Select();
            CommandConsoleEvents.InvokeOpen();
        }

        public void DisableConsole(GameObject console, TMP_InputField inputField)
        {
            console.SetActive(false);
            model.IsConsoleEnabled = false;
            
            inputField.text = string.Empty;
            StartCoroutine(SetCaretPosition(inputField, 0));
            CommandConsoleEvents.InvokeClose();
        }

        /// <summary> Simulate CTRL + Backspace </summary>
        public void DeleteTextBlock(TMP_InputField inputField)
        {
            if (!model.IsConsoleEnabled) return;
    
            string text = inputField.text;
            int caretPosition = inputField.caretPosition;
    
            if (caretPosition > 0 && caretPosition <= text.Length)
            {
                int startIndex = caretPosition - 1;

                while (startIndex > 0 && char.IsWhiteSpace(text[startIndex]))
                {
                    startIndex--;
                }

                while (startIndex > 0 && !char.IsWhiteSpace(text[startIndex - 1]))
                {
                    startIndex--;
                }

                bool hasTrailingSpace = startIndex > 0 && char.IsWhiteSpace(text[startIndex - 1]);

                string newText = text.Remove(startIndex, caretPosition - startIndex);
        
                if (hasTrailingSpace)
                {
                    newText = newText.Insert(startIndex, " ");
                }

                inputField.text = newText;
                inputField.caretPosition = startIndex + (hasTrailingSpace ? 1 : 0);
            }
        }
        
        /// <summary> Hack to properly set caret position </summary>
        static IEnumerator SetCaretPosition(TMP_InputField inputField, int index)
        {
            int width = inputField.caretWidth;
            inputField.caretWidth = 0;

            yield return new WaitForEndOfFrame();
            
            inputField.caretWidth = width;
            inputField.caretPosition = index;
        }

        public void SetScrollRectPosition(ScrollRect scrollRect, float height)
        {
            StartCoroutine(SetScrollRectPositionRoutine(scrollRect, height));
        }
        
        IEnumerator SetScrollRectPositionRoutine(ScrollRect scrollRect, float height)
        {
            yield return new WaitForEndOfFrame();
            scrollRect.verticalNormalizedPosition = height;
        }
        
        public void CreateAndInitializeNewLog(Transform messagesRoot, string message, string stackTrace, LogType logType)
        {
            // todo: object pool visible logs

            MessageKey key = new MessageKey(message, logType);
            bool containsKey = model.Logs.TryGetValue(key, out ConsoleMessage consoleMessage);
            if (model.CollapseMessages && containsKey)
            {
                consoleMessage.IncrementMessageCount();
                return;
            }
            
            consoleMessage = Instantiate(model.ConsoleMessagePrefab, messagesRoot);
            consoleMessage.Initialize(key);

            if(!containsKey) model.Logs.Add(key, consoleMessage);
        }

        static string GetTypeName(Type type)
        {
            return type switch
            {
                not null when type == typeof(float) => "float",
                not null when type == typeof(bool) => "bool",
                not null when type == typeof(int) => "int",
                not null when type == typeof(string) => "string",
                not null when type == typeof(Vector3) => "Vector3",
                not null when type == typeof(Vector2) => "Vector2",
                not null when type == typeof(Vector2Int) => "Vector2Int",
                not null when type == typeof(Vector3Int) => "Vector3Int",
                not null when type == typeof(long) => "long",
                not null when type == typeof(ulong) => "ulong",
                not null when type == typeof(double) => "double",
                not null when type == typeof(byte) => "byte",
                not null when type == typeof(sbyte) => "sbyte",
                not null when type == typeof(short) => "short",
                not null when type == typeof(char) => "char",
                _ => INVALID_PARAMETER
            };
        }
        
        static object GetParsedArg(Type type, string arg)
        {
            if (type == typeof(string)) return arg;
            if(type == typeof(float) && float.TryParse(arg, out float floatVal)) return floatVal;
            if(type == typeof(int) && int.TryParse(arg, out int intVal)) return intVal;
            if(type == typeof(bool) && bool.TryParse(arg, out bool boolVal)) return boolVal;
            if (type.IsEnum && Enum.TryParse(type, arg, true, out object enumVal)) return enumVal;
            
            // split the arg by commas. example valid vector3 arg: "3.2,1.5"
            string[] commaSplitValues = arg.Split(",");
            if(type == typeof(Vector2) && commaSplitValues.Length == 2)
            {
                if (float.TryParse(commaSplitValues[0], out float f1) && float.TryParse(commaSplitValues[1], out float f2))
                    return new Vector2(f1, f2);
            }
            if(type == typeof(Vector3) && commaSplitValues.Length == 3)
            {
                if (float.TryParse(commaSplitValues[0], out float f1) && float.TryParse(commaSplitValues[1], out float f2) && float.TryParse(commaSplitValues[2], out float f3))
                    return new Vector3(f1, f2, f3);
            }
            if(type == typeof(Vector2Int) && commaSplitValues.Length == 2)
            {
                if (int.TryParse(commaSplitValues[0], out int i1) && int.TryParse(commaSplitValues[1], out int i2))
                    return new Vector2Int(i1, i2);
            }
            if(type == typeof(Vector3Int) && commaSplitValues.Length == 3)
            {
                if (int.TryParse(commaSplitValues[0], out int i1) && int.TryParse(commaSplitValues[1], out int i2) && int.TryParse(commaSplitValues[2], out int i3))
                    return new Vector3Int(i1, i2, i3);
            }
            
            if(type == typeof(long) && long.TryParse(arg, out long longVal)) return longVal;
            if(type == typeof(ulong) && ulong.TryParse(arg, out ulong ulongVal)) return ulongVal;
            if(type == typeof(double) && double.TryParse(arg, out double doubleVal)) return doubleVal;
            if(type == typeof(byte) && byte.TryParse(arg, out byte byteVal)) return byteVal;
            if(type == typeof(sbyte) && sbyte.TryParse(arg, out sbyte sbyteVal)) return sbyteVal;
            if(type == typeof(short) && short.TryParse(arg, out short shortVal)) return shortVal;

            return null;
        }

        public void ToggleMessageCollapsing(bool value)
        {
            model.CollapseMessages = value;
            model.SetSavedCollapseMessagesValue();
        }

        public void UpdatePlaceholderText(TMP_Text placeholderText, TMP_InputField input, bool ignoreStringCheck = false)
        {
            model.SelectedCommand = CommandKey.empty;
            placeholderText.SetText(string.Empty);
            
            foreach ((CommandKey key, MethodInfo methodInfo) in model.RegisteredCommands)
            {
                string commandName = key.name;

                string[] splitInputString = input.text.Split(new[] { ' ' }, 2);
                string inputCommandName = splitInputString[0];

                if (!commandName.ToLower().StartsWith(inputCommandName.ToLower())
                    || (input.text.Length > commandName.Length && !key.HasParameters)
                    || (commandName != inputCommandName && input.text.Contains(" ")))
                {
                    continue;
                }

                ParameterInfo[] methodParams = methodInfo.GetParameters();
                
                string parameters = string.Empty;
                foreach (ParameterInfo param in methodParams)
                {
                    parameters += $" {param.Name}";
                }

                const string colorStart = "<color=#CC7744>";
                const string colorEnd = "</color>";

                string placeHolderString = $"{key.name}";
                string combinedParamsString = splitInputString.Length > 1 ? splitInputString[1] : string.Empty;

                if (key.HasParameters && splitInputString.Length > 1 && combinedParamsString.Trim() != string.Empty)
                {
                    string[] inputParameters = combinedParamsString.Split(' ');

                    int inputParamsCount = inputParameters.Length;
                    placeHolderString += " " + combinedParamsString;
                    if (inputParameters.Last().Trim() == string.Empty)
                    {
                        inputParamsCount--;
                        placeHolderString = placeHolderString.Remove(input.caretPosition-1, 1);
                    }
                    for (int i = inputParamsCount; i < methodParams.Length; i++)
                    {
                        placeHolderString += $" {colorStart}{methodParams[i].Name}{colorEnd}";
                    }
                }
                else
                {
                    placeHolderString += colorStart + parameters + colorEnd;
                }
                
                placeholderText.SetText(placeHolderString);

                model.SelectedCommand = key;
            }
            
            if (input.text == string.Empty)
            {
                placeholderText.SetText(string.Empty);
            }
        }
        
        public void FillAutoCompletedText(TMP_InputField inputField)
        {
            bool isTypingParams = inputField.text.Split(" ", 2).Length == 1;
            if (model.IsConsoleEnabled && isTypingParams)
            {
                inputField.text = model.SelectedCommand.name;
                StartCoroutine(SetCaretPosition(inputField, inputField.text.Length));
            }
        }

        public void ClearMessages(Transform messagesRoot)
        {
            foreach (KeyValuePair<MessageKey, ConsoleMessage> log in model.Logs)
            {
                if(log.Value != null) Destroy(log.Value.gameObject);
            }

            foreach (Transform message in messagesRoot)
            {
                Destroy(message.gameObject);
            }
            
            model.Logs.Clear();
        }

        public void SubmitCommand(TMP_InputField textInput, TMP_Text inputPlaceholderText, ScrollRect messagesScrollRect, RectTransform panelRoot)
        {
            InvokeCommand(textInput);
            SetScrollRectPosition(messagesScrollRect, 0);
            UpdatePlaceholderText(inputPlaceholderText, textInput);
        }
    }
}