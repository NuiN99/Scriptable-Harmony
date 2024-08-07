﻿using System;
using JetBrains.Annotations;

namespace NuiN.CommandConsole
{
#if RIDER
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
#endif
    [AttributeUsage(AttributeTargets.Method)]
    public class ConsoleCommandAttribute : Attribute
    {
        public readonly string command;
    
        public ConsoleCommandAttribute(string command)
        {
            this.command = command;
        }
    }
}