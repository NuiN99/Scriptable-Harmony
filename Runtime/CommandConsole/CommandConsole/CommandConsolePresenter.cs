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
        
        [Conditional("UNITY_EDITOR")]
        public void RegisterAssemblies()
        {
            List<string> registeredAssemblies = model.RegisteredAssemblies;
            if (registeredAssemblies == null) return;
            registeredAssemblies.Clear();
                
            // Assembly-CSharp doesn't exist when no scripts are using it
            const string assemblyCSharpPath = "Library/ScriptAssemblies/Assembly-CSharp.dll";
            if (File.Exists(assemblyCSharpPath))
            {
                registeredAssemblies.Add("Assembly-CSharp");
            }
                
            string[] guids = AssetDatabase.FindAssets("t:asmdef", new[] { "Assets" });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                    
                // editor assemblies don't exist in the build and throw errors
                if (path.Contains("/Editor/"))
                {
                    continue;
                }
                    
                string assetName = Path.GetFileNameWithoutExtension(path);
                registeredAssemblies.Add(assetName);
            }
        }
        
        public void RegisterCommands()
        {
            model.RegisteredCommands = new Dictionary<string, MethodInfo>();

            List<Assembly> loadedAssemblies = model.RegisteredAssemblies.Select(Assembly.Load).ToList();
                
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
            if (model.InitialScalePos == Vector2.zero) model.InitialScalePos = rectTransform.position;
            if (model.InitialScale != Vector2.zero)  model.InitialScale = rectTransform.sizeDelta;

            Vector2 newScale =  (model.InitialScale + ((Vector2)Input.mousePosition - model.InitialScalePos)) - pressOffset;
            newScale.x = Mathf.Clamp(newScale.x, model.MinScale.x, model.MaxScale.x);
            newScale.y = Mathf.Clamp(newScale.y, model.MinScale.y, model.MaxScale.y);
                    
            rectTransform.sizeDelta = newScale;
        }

        public void UpdatePosition(RectTransform rectTransform)
        {
            if (model.InitialMovePos == Vector2.zero) model.InitialMovePos = Input.mousePosition - rectTransform.position;

            float maxX = Screen.width - rectTransform.sizeDelta.x;
            float maxY = Screen.height - rectTransform.sizeDelta.y;
                    
            Vector2 newPosition = (Vector2)Input.mousePosition - model.InitialMovePos;
            newPosition.x = Mathf.Clamp(newPosition.x, 0, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, 0, maxY);

            rectTransform.position = newPosition;
        }

        public void ResetInitialSizeValues()
        {
            model.InitialScalePos = Vector2.zero;
            model.InitialScale = Vector2.zero;
        }

        public void ResetInitialPositionValues()
        {
            model.InitialMovePos = Vector2.zero;
        }
    }
}