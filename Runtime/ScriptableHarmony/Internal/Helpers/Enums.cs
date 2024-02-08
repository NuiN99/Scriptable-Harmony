namespace NuiN.ScriptableHarmony.Core
{
    public enum Access { Getter, Setter }
    public enum SOType { Variable, List, Dictionary, RuntimeSet, RuntimeSingle }
    public enum RuntimeOptions { ResetOnSceneLoad, InvokeOnSceneLoad, None }
    public enum RuntimeSetLifetime { OnAwakeOnDestroy, OnEnableOnDisable, RemoveOnDestroy, RemoveOnDestroyAndDisable }
}

