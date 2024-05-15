using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NuiN.NExtensions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace NuiN.CommandConsole
{
    public class CommandConsole : MonoBehaviour
    {
        [SerializeField] RectTransform panelRoot;
        [SerializeField] TMP_InputField textInput;
        [SerializeField] PointerButton scaleButton;
        [SerializeField] PointerButton moveButton;

        [SerializeField] Vector2 minScale = new(200, 125);
        [SerializeField] Vector2 maxScale = new(1920, 1080);
        
        [SerializeField, ReadOnly] List<string> commandAssemblies = new();

        Dictionary<string, MethodInfo> _registeredCommands = new();

        Vector2 _initialMovePos;
        Vector2 _initialScalePos;
        Vector2 _initialScale;
        
    #if UNITY_EDITOR
        void OnValidate()
        {
            if (commandAssemblies == null) return;
            commandAssemblies.Clear();
            
            // Assembly-CSharp doesn't exist when no scripts are using it
            const string assemblyCSharpPath = "Library/ScriptAssemblies/Assembly-CSharp.dll";
            if (File.Exists(assemblyCSharpPath))
            {
                commandAssemblies.Add("Assembly-CSharp");
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
                commandAssemblies.Add(assetName);
            }
            
            EditorUtility.SetDirty(this);
        }
    #endif

        void Awake() => RegisterCommandAttributeMethods();
        void OnEnable() => textInput.onSubmit.AddListener(InvokeCommand);
        void OnDisable() => textInput.onSubmit.RemoveListener(InvokeCommand);
        void Update() => MoveAndScalePanel();

        void RegisterCommandAttributeMethods()
        {
            _registeredCommands = new Dictionary<string, MethodInfo>();

            List<Assembly> loadedAssemblies = commandAssemblies.Select(Assembly.Load).ToList();
            
            foreach (var assembly in loadedAssemblies)
            {
                foreach (var type in assembly.GetTypes().Where(type => typeof(MonoBehaviour).IsAssignableFrom(type)))
                {
                    foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                    {
                        var commandAttributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
                        foreach (CommandAttribute attribute in commandAttributes)
                        {
                            if (!method.IsStatic && !typeof(MonoBehaviour).IsAssignableFrom(method.DeclaringType)) continue;
                            
                            if (!_registeredCommands.TryAdd(attribute.command, method))
                            {
                                Debug.LogWarning($"Command already declared for [{attribute.command}] in [{method.DeclaringType}]");
                            }
                        }
                    }
                }
            }
        }

        void MoveAndScalePanel()
        {
            if (scaleButton.Pressed)
            {
                if (_initialScalePos == Vector2.zero) _initialScalePos = panelRoot.position;
                if (_initialScale != Vector2.zero)  _initialScale = panelRoot.sizeDelta;

                Vector2 newScale =  (_initialScale + ((Vector2)Input.mousePosition - _initialScalePos)) - scaleButton.PressOffset;
                newScale.x = Mathf.Clamp(newScale.x, minScale.x, maxScale.x);
                newScale.y = Mathf.Clamp(newScale.y, minScale.y, maxScale.y);
                
                panelRoot.sizeDelta = newScale;
            }
            else
            {
                _initialScale = Vector2.zero;
                _initialScalePos = Vector2.zero;
            }

            if (moveButton.Pressed)
            {
                if (_initialMovePos == Vector2.zero) _initialMovePos = Input.mousePosition - panelRoot.position;

                float maxX = Screen.width - panelRoot.sizeDelta.x;
                float maxY = Screen.height - panelRoot.sizeDelta.y;
                
                Vector2 newPosition = (Vector2)Input.mousePosition - _initialMovePos;
                newPosition.x = Mathf.Clamp(newPosition.x, 0, maxX);
                newPosition.y = Mathf.Clamp(newPosition.y, 0, maxY);

                panelRoot.position = newPosition;
            }
            else
            {
                _initialMovePos = Vector2.zero;
            }
        }

        void InvokeCommand(string fullCommand)
        {
            // reselect the input field and move the caret to the end for good UX
            textInput.ActivateInputField();
            textInput.caretPosition = fullCommand.Length;
            
            // no input was detected
            if (fullCommand.Trim().Length <= 0) return;
            
            // the first space after the method name indicates that parameters have been entered
            string[] commandParts = fullCommand.Split(new[] { ' ' }, 2);
            string commandName = commandParts[0];
            
            if (!_registeredCommands.TryGetValue(commandName, out MethodInfo method))
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
            
            textInput.SetTextWithoutNotify(string.Empty);
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

        [Command("add")]
        void TestCommand(int num1, float num2 = 4)
        {
            Debug.LogError(num1 + num2); 
        }

        [Command("scene.load")]
        void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }
        
        [Command("scene.reload")]
        void ReloadScene()
        {
            GeneralUtils.ReloadScene();
        }
    }
}