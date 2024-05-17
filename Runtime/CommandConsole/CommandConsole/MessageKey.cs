using System.Collections.Generic;
using UnityEngine;

namespace NuiN.CommandConsole
{
    public readonly struct MessageKey : IEqualityComparer<MessageKey>
    {
        public readonly string message;
        public readonly LogType logType;

        public MessageKey(string message, LogType logType)
        {
            this.message = message;
            this.logType = logType;
        }

        public bool Equals(MessageKey x, MessageKey y)
        {
            bool sameMessage = x.message == y.message;
            bool sameLogType = x.logType == y.logType;
            return (sameMessage && sameLogType);
        }

        public int GetHashCode(MessageKey obj)
        {
            // prevent integer overflow
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + obj.message.GetHashCode();
                hash = hash * 31 + logType.GetHashCode();

                return hash;
            }
        }
        
        public override int GetHashCode() => GetHashCode(this);
    }
}