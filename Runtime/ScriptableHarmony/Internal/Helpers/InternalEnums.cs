namespace NuiN.ScriptableHarmony.Internal.Helpers
{
    internal enum Access { Getter, Setter }
    internal enum SOType { Variable, List, Dictionary, RuntimeSet, RuntimeSingle }
    internal enum ResetOn { SceneLoad, ExitPlayMode }
    internal enum LifetimeType { OnEnableOnDisable, OnAwakeOnDestroy, OnlyRemoveOnDestroy, OnlyRemoveOnDestroyAndDisable }
}

