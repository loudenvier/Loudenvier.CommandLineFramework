using Loudenvier.CommandLineFramework;

var loop = new CommandLoop();
var year = DateTime.Now.Year;
loop.Printer.Attention($"""
    Command Line Framework - Sample App
    (C) 2024{(year > 2024 ? $"-{year}" : "")} by Felipe Machado (Loudenvier)

    """);
await loop.RunAsync();

static class TestComandSet
{

}
