using System;

[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute : Attribute
{
    public readonly string command;
    
    public CommandAttribute(string command)
    {
        this.command = command;
    }
}