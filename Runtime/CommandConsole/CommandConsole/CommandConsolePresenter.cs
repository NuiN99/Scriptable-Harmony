using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace NuiN.CommandConsole
{
    public class CommandConsolePresenter : MonoBehaviour
    {
        [SerializeField] CommandConsoleModel model;
        
        public void RegisterAssemblies()
        {
            if(model.AssemblyContainer != null) model.AssemblyContainer.FindAndRegister();
        }

        public void LoadSavedScaleAndPosition(RectTransform root)
        {
            model.ConsolePosition = root.position;
            model.ConsoleSize = root.sizeDelta;
            root.position = model.GetSavedPosition();
            root.sizeDelta = model.GetSavedSize();
        }
        
        public void RegisterCommands()
        {
            model.RegisteredCommands = new Dictionary<string, MethodInfo>();

            List<Assembly> loadedAssemblies = model.AssemblyContainer.RegisteredAssemblies.Select(Assembly.Load).ToList();
                
            foreach (var assembly in loadedAssemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                    {
                        var commandAttributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
                        foreach (CommandAttribute attribute in commandAttributes)
                        {
                            if (!method.IsStatic && !typeof(MonoBehaviour).IsAssignableFrom(method.DeclaringType)) continue;
                                
                            if (!model.RegisteredCommands.TryAdd(attribute.command, method))
                            {
                                Debug.LogWarning($"Command already declared for [{attribute.command}] in [{method.DeclaringType}]");
                            }
                        }
                    }
                }
            }
        }
        
        public void InvokeCommand(string fullCommand, TMP_InputField inputField)
        {
            // reselect the input field and move the caret to the end for good UX
            inputField.ActivateInputField();
            inputField.caretPosition = fullCommand.Length;
            
            // no input was detected
            if (fullCommand.Trim().Length <= 0) return;
            
            // the first space after the method name indicates that parameters have been entered
            string[] commandParts = fullCommand.Split(new[] { ' ' }, 2);
            string commandName = commandParts[0];
            
            if (!model.RegisteredCommands.TryGetValue(commandName, out MethodInfo method))
            {
                Debug.Log("Command not found...");
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
                    
                    object param = ParseParameter(stringParam, parameterInfo.ParameterType);
                    
                    if (param == null)
                    {
                        Debug.LogError("Invalid Parameter");
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

            object[] paramsArray = parameters.ToArray();
            
            if (method.IsStatic)
            {
                method.Invoke(null, paramsArray);
            }
            else
            {
                // find and invoke all instances of the method's class in the scene
                Object[] classInstances = FindObjectsByType(method.DeclaringType, FindObjectsSortMode.None);
                if (classInstances.Length <= 0)
                {
                    Debug.LogError("No instances found to run the command");
                }
                foreach (var instance in classInstances)
                {
                    method.Invoke(instance, paramsArray);
                }
            }
            
            inputField.SetTextWithoutNotify(string.Empty);
        }
        
        object ParseParameter(string param, Type paramType)
        {
            object value = null;
            if (paramType == typeof(int) && int.TryParse(param, out int intValue))
            {
                value = intValue;
            }
            else if (paramType == typeof(float) && float.TryParse(param, out float floatValue))
            {
                value = floatValue;
            }
            else if (paramType == typeof(bool) && bool.TryParse(param, out bool boolValue))
            {
                value = boolValue;
            }
            else if (paramType == typeof(string))
            {
                value = param;
            }

            return value;
        }
        
        bool HasValidParameterCount(int inputCount, int minCount, int maxCount)
        {
            if (inputCount < minCount)
            {
                Debug.LogError("Not enough parameters!");
                return false;
            }
            if (inputCount > maxCount)
            {
                Debug.LogError("Too many parameters!");
                return false;
            }

            return true;
        }

        public void UpdateSize(RectTransform rectTransform, Vector2 pressOffset)
        {
            // prevent scaling outside of screen
            Vector2 mousePosition = (Vector2)Input.mousePosition - pressOffset;
            mousePosition.x = Mathf.Clamp(mousePosition.x, 0, Screen.width);
            mousePosition.y = Mathf.Clamp(mousePosition.y, 0, Screen.height);
            
            if (model.InitialScalePos == Vector2.zero) model.InitialScalePos = rectTransform.position;
            if (model.InitialScale != Vector2.zero)  model.InitialScale = rectTransform.sizeDelta;

            Vector2 newSize =  (model.InitialScale + (mousePosition - model.InitialScalePos));
            newSize.x = Mathf.Clamp(newSize.x, model.MinSize.x, model.MaxScale.x);
            newSize.y = Mathf.Clamp(newSize.y, model.MinSize.y, model.MaxScale.y);
                    
            rectTransform.sizeDelta = newSize;
            model.ConsoleSize = newSize;
        }

        public void UpdatePosition(RectTransform rectTransform)
        {
            if (model.InitialMovePos == Vector2.zero) model.InitialMovePos = Input.mousePosition - rectTransform.position;
            
            Vector2 newPosition = (Vector2)Input.mousePosition - model.InitialMovePos;
            Vector2 maxPosition = new(Screen.width - rectTransform.sizeDelta.x, Screen.height - rectTransform.sizeDelta.y);
            newPosition.x = Mathf.Clamp(newPosition.x, 0, maxPosition.x);
            newPosition.y = Mathf.Clamp(newPosition.y, 0, maxPosition.y);

            rectTransform.position = newPosition;
            model.ConsolePosition = newPosition;
        }

        public void ResetInitialSizeValues()
        {
            model.InitialScalePos = Vector2.zero;
            model.InitialScale = Vector2.zero;
            
            model.SetSavedScale();
        }

        public void ResetInitialPositionValues()
        {
            model.InitialMovePos = Vector2.zero;
            
            model.SetSavedPosition();
        }
    }
}