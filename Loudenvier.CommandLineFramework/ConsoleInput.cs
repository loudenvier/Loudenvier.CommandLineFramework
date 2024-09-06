namespace Loudenvier.CommandLineFramework;

public readonly record struct ConsoleInput(
    InputType Type, 
    string Command = "", 
    string CommandData = "")
{
    public static readonly ConsoleInput Exit = new(InputType.Exit);
};

public enum InputType { Nothing, Command, Exit };


