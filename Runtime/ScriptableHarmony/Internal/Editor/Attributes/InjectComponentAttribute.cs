using System;

namespace NuiN.CommandConsole
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InjectComponentAttribute : Attribute { }
}