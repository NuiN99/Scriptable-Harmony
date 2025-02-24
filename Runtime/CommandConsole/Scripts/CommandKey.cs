using System;
using System.Collections.Generic;
using System.Reflection;

namespace NuiN.CommandConsole
{
    public readonly struct CommandKey : IEqualityComparer<CommandKey>, IEquatable<CommandKey>, IComparable<CommandKey>
    {
        public static CommandKey empty = new(string.Empty, new List<ParameterInfo>());
        public bool HasParameters => parameterTypes.Count > 0;
        
        public readonly string name;
        public readonly List<Type> parameterTypes;

        public CommandKey(string name, IEnumerable<ParameterInfo> parameterInfos)
        {
            parameterTypes = new List<Type>();
            this.name = name;
            foreach (var param in parameterInfos)
            {
                parameterTypes.Add(param.ParameterType);
            }
        }
        
        public bool Equals(CommandKey x, CommandKey y)
        {
            if (x.name != y.name) return false;
            
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
                hash = hash * 31 + obj.name.GetHashCode();

                foreach (var type in obj.parameterTypes)
                {
                    hash = hash * 31 + (type != null ? type.GetHashCode() : 0);
                }

                return hash;
            }
        }
        
        public override int GetHashCode() => GetHashCode(this);

        public bool Equals(CommandKey other)
        {
            return name == other.name;
        }

        public override bool Equals(object obj)
        {
            return obj is CommandKey other && Equals(other);
        }

        public int CompareTo(CommandKey other)
        {
            return string.Compare(name, other.name, StringComparison.Ordinal);
        }
    }
}