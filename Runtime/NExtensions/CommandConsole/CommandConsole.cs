using System;
using System.Collections.Generic;
using System.Reflection;
using NuiN.NExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

public class CommandConsole : MonoBehaviour
{
    [SerializeField] TMP_InputField textInput;

    static Dictionary<string, MethodInfo> registeredCommands = new();

    static bool registrationComplete;

    [RuntimeInitializeOnLoadMethod]
    static void RegisterCommands()
    {
        registeredCommands = new Dictionary<string, MethodInfo>();
        registrationComplete = false;
        
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    var commandAttributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
                    foreach (CommandAttribute attribute in commandAttributes)
                    {
                        if (method.IsStatic || typeof(MonoBehaviour).IsAssignableFrom(method.DeclaringType))
                        {
                            if (!registeredCommands.TryAdd(attribute.command, method))
                            {
                                Debug.LogWarning($"Command already declared for [{attribute.command}] in [{method.DeclaringType}]");
                            }
                        }
                    }
                }
            }
        }

        registrationComplete = true;
    }

    void OnEnable()
    {
        textInput.onSubmit.AddListener(InvokeCommand);
    }
    void OnDisable()
    {
        textInput.onSubmit.RemoveListener(InvokeCommand);
    }

    void InvokeCommand(string command)
    {
        textInput.ActivateInputField();
        textInput.caretPosition = command.Length;

        if (!registrationComplete)
        {
            Debug.LogError("Command registration still in progress, please try again later");
            return;
        }

        if (!registeredCommands.TryGetValue(command, out MethodInfo method))
        {
            Debug.Log("Command not found");
            return;
        }

        if (method.IsStatic)
        {
            method.Invoke(null, null);
        }
        else
        {
            Object[] classInstances = FindObjectsByType(method.DeclaringType, FindObjectsSortMode.None);
            foreach (var instance in classInstances)
            {
                method.Invoke(instance, null);
            }
        }
        
        textInput.SetTextWithoutNotify(string.Empty);
    }

    static MethodInfo GetCommandMethod(string command)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    var commandAttributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
                    foreach (CommandAttribute attribute in commandAttributes)
                    {
                        if (attribute.command.Equals(command) && (method.IsStatic || typeof(MonoBehaviour).IsAssignableFrom(method.DeclaringType)))
                        {
                            return method;
                        }
                    }
                }
            }
        }
        
        return null;
    }

    [Command("test.command")]
    void TestCommand()
    {
        Debug.Log("Test Command Success!"); 
    }
}