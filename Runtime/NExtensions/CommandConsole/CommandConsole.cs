using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NuiN.NExtensions;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

public class CommandConsole : MonoBehaviour
{
    [SerializeField] TMP_InputField textInput;
    
    #if UNITY_EDITOR
    [SerializeField] AssemblyDefinitionAsset[] commandAssemblies;
    #endif
    
    [SerializeField, ReadOnly] List<string> commandAssemblyNames = new();

    Dictionary<string, MethodInfo> _registeredCommands = new();
    
#if UNITY_EDITOR
    void OnValidate()
    {
        if (commandAssemblyNames == null) return;
        commandAssemblyNames.Clear();
        
        foreach (AssemblyDefinitionAsset assemblyAsset in commandAssemblies.Reverse())
        {
            if (assemblyAsset == null)
            {
                continue;
            }
            commandAssemblyNames.Add(assemblyAsset.name);
        }
    }
#endif

    void OnEnable()
    {
        textInput.onSubmit.AddListener(InvokeCommand);
    }
    void OnDisable()
    {
        textInput.onSubmit.RemoveListener(InvokeCommand);
    }

    void Awake()
    {
        _registeredCommands = new Dictionary<string, MethodInfo>();

        List<Assembly> loadedAssemblies = commandAssemblyNames.Select(Assembly.Load).ToList();
        
        foreach (var assembly in loadedAssemblies)
        {
            foreach (var type in assembly.GetTypes().Where(type => typeof(MonoBehaviour).IsAssignableFrom(type)))
            {
                foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    var commandAttributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
                    foreach (CommandAttribute attribute in commandAttributes)
                    {
                        if (method.IsStatic || typeof(MonoBehaviour).IsAssignableFrom(method.DeclaringType))
                        {
                            if (!_registeredCommands.TryAdd(attribute.command, method))
                            {
                                Debug.LogWarning($"Command already declared for [{attribute.command}] in [{method.DeclaringType}]");
                            }
                        }
                    }
                }
            }
        }
    }

    void InvokeCommand(string fullCommand)
    {
        if (fullCommand.Trim().Length <= 0) return;
        
        string[] commandParts = fullCommand.Split(new[] { ' ' }, 2);
        string command = commandParts[0];
        
        if (!_registeredCommands.TryGetValue(command, out MethodInfo method))
        {
            Debug.Log("Command not found...");
            return;
        }
        
        object[] parameters = {};
        ParameterInfo[] parameterInfos = method.GetParameters();

        bool hasParameters = commandParts.Length > 1;
        if (hasParameters)
        {
            string[] stringParameters = commandParts[1].Split(" ");
            parameters = new object[stringParameters.Length];

            
            if (parameterInfos.Length != parameters.Length)
            {
                Debug.LogError("Incorrect parameter count");
                return;
            }
            
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                ParameterInfo parameterInfo = parameterInfos[i];
                string stringParam = stringParameters[i];

                object param = ParseParameter(stringParam, parameterInfo.ParameterType);
                if (param == null)
                {
                    Debug.LogError("Invalid Parameter");
                    return;
                }
                
                parameters[i] = param;
            }
        }
        
        if (parameterInfos.Length != parameters.Length)
        {
            Debug.LogError("Incorrect parameter count");
            return;
        }
        
        textInput.ActivateInputField();
        textInput.caretPosition = fullCommand.Length;
        
        if (method.IsStatic)
        {
            method.Invoke(null, parameters);
        }
        else
        {
            Object[] classInstances = FindObjectsByType(method.DeclaringType, FindObjectsSortMode.None);
            foreach (var instance in classInstances)
            {
                method.Invoke(instance, parameters);
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

        return value;
    }

    [Command("test")]
    static void TestCommand(int num1, float num2 = 1)
    {
        Debug.Log(num1 + num2); 
    }
}