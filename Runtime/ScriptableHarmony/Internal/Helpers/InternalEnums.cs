namespace NuiN.ScriptableHarmony.Internal.Helpers
{
    internal enum Access { Getter, Setter }
    internal enum SOType { Variable, ListVariable, DictionaryVariable, RuntimeSet, RuntimeSingle }
    internal enum ResetOn { ExitPlayMode, SceneLoad }
    internal enum LifetimeType { OnEnableOnDisable, OnAwakeOnDestroy, OnlyRemoveOnDestroy, OnlyRemoveOnDestroyAndDisable }
}

