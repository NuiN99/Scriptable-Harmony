using System;
using System.Collections.Generic;
using System.Reflection;

namespace NuiN.CommandConsole
{
    public readonly struct CommandKey : IEqualityComparer<CommandKey>
    {
        public readonly string command;
        public readonly List<Type> parameterTypes;

        public CommandKey(string command, List<ParameterInfo> parameterInfos)
        {
            parameterTypes = new List<Type>();
            this.command = command;
            foreach (var param in parameterInfos)
            {
                parameterTypes.Add(param.ParameterType);
            }
        }
        
        public bool Equals(CommandKey x, CommandKey y)
        {
            if (x.command != y.command) return false;
            
            if (x.parameterTypes.Count != y.parameterTypes.Count) return false;
            
            for (int i = 0; i < x.parameterTypes.Count; i++)
            {
                if (x.parameterTypes[i] != y.parameterTypes[i]) return false;
            }
            
            return true;
        }

        public int GetHashCode(CommandKey obj)
        {
            // prevent integer overflow
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + obj.command.GetHashCode();

                foreach (var type in obj.parameterTypes)
                {
                    hash = hash * 31 + (type != null ? type.GetHashCode() : 0);
                }

                return hash;
            }
        }
        
        public override int GetHashCode() => GetHashCode(this);
    }
}