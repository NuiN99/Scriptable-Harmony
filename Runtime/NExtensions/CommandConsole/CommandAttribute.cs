using System;
using JetBrains.Annotations;

namespace NuiN.CommandConsole
{
#if RIDER
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
#endif
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public readonly string command;
    
        public CommandAttribute(string command)
        {
            this.command = command;
        }
    }
}