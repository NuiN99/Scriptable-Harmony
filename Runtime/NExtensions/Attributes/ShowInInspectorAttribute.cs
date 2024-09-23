using System;

namespace NuiN.NExtensions
{
    // modified from https://github.com/BrainswitchMachina/Show-In-Inspector
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ShowInInspectorAttribute : Attribute { }
}