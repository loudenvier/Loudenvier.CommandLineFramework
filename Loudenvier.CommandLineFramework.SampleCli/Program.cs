using Loudenvier.CommandLineFramework;

var loop = new CommandLoop();
loop.CommandSet.AutoRegisterCommands();

var year = DateTime.Now.Year;
loop.Printer.Attention($"""
    Command Line Framework - Sample App
    (C) 2024{(year > 2024 ? $"-{year}" : "")} by Felipe Machado (Loudenvier)

    """);
loop.Printer.Error("The command failed with error:", "Command error!", msgColor: ConsoleColor.Yellow);
await loop.RunAsync();

static class TestComandSet
{

}
