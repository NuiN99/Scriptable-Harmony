namespace NuiN.ScriptableHarmony.Core
{
    internal enum Access { Getter, Setter }
    internal enum SOType { Variable, List, Dictionary, RuntimeSet, RuntimeSingle }
    internal enum ResetOn { SceneLoad, ExitPlayMode }
    internal enum LifetimeType { OnAwakeOnDestroy, OnEnableOnDisable, RemoveOnDestroy, RemoveOnDestroyAndDisable }
}

