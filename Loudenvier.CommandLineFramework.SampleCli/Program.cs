using CommandLine;
using Loudenvier.CommandLineFramework;

var loop = new CommandLoop(commandSet: new(autoRegisterCommands: true));
loop.CommandSet.RegisterCommandSet<ExplictCommands>();

loop.Printer.Attention("Loudenvier's Command Line Framework Hello [World]\r\n");
await loop.RunAsync();

static class AutoCommandSet // automatically discovered when auto registration is on
{
    class HelloWorldOptions // implicitly discovered as HelloWorldCommand options
    {
        [Value(0, MetaName = "name", Default = "World", HelpText = "To whom we'll say hello.")]
        public string? Name { get; set; }
    }
    // HelloWorld is "auto-registered" as the command "HelloWorld" due to the default conventions
    static void HelloWorldCommand(HelloWorldOptions o, CommandLoop loop) 
        => loop.Printer.Resp($"Hello {o.Name}!");
}

class ExplictCommands // added manually with RegisterCommandSet<T>
{
    [Command("test")] // commands can be added explicitly via the Command attribute
    static void Test() => Console.WriteLine("Test was called");
}