using System;

namespace NuiN.ScriptableHarmony.Events
{
    public static class VariableEvents
    {
        public static event Action OnResetAllVariableObjects;
        public static void ResetAllVariableObjects() => OnResetAllVariableObjects?.Invoke();
    }
}