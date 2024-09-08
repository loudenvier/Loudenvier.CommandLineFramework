# Loudenvier.CommandLineFramework
A framework to help implementing Command Line Interface (CLI) programs based on textual commands. It has rich editing capabilities and command history support. Adding new commands is as simple as writing a static method!

> **NOTE**: _This project is in a very early alpha stage. It's already being used in my own production code but be aware that ***BREAKING CHANGES will*** occur._

This is all the code you need to write a simple CLI program with built-in commands, command history, autocompletion and a custom "HelloWorld" command:
```csharp
using CommandLine;
using Loudenvier.CommandLineFramework;

await new CommandLoop(commandSet: new(autoRegisterCommands: true)).RunAsync();

static class AutoCommandSet // automatically discovered when auto registration is on
{
    // HelloWorld is "auto-registered" as the command "HelloWorld" due to the default conventions
    static void HelloWorldCommand(HelloWorldOptions o, CommandLoop loop)
        => loop.Printer.Resp($"Hello {o.Name}!");

    class HelloWorldOptions // implicitly discovered as HelloWorldCommand options
    {
        [Value(0, MetaName = "name", Default = "World", HelpText = "To whom we'll say hello.")]
        public string? Name { get; set; }
    }
}
```

This framework differ somewhat from other .NET console libraries as its main focus is on registering and running commands typed by the user: it implements easy registration of new commands, automatic discovery of commands based on (customizable) conventions, mapping of user input to specific commands while automatically parsing arguments, autocompletion of user input, etc.

In many ways it is more akin to Python's [cmd package](https://docs.python.org/3/library/cmd.html) (which I didn't know until one day before the initial commit of this library :-) than .NET libraries like Spectre.Console and PrettyPrompt. In fact, I've found it through the [cmd2 package](https://cmd2.readthedocs.io/en/stable/), which inspired me to change the way commands are defined from using object hierarquies to using plain static methods.

This library currently (ab)uses [Eric Newton](https://github.com/ericnewton76)'s amazing [Command Line Parser](https://github.com/commandlineparser/commandline) for parsing command arguments and generating command and options help, as it is a stable, proven and feature-rich library, which I have been using for ages and for which I'm very thankful. While there are motivations to migrate to [System.CommandLine](https://learn.microsoft.com/en-us/dotnet/standard/commandline/), which is an "official" Microsoft solution for parsing commands, generating help, etc. it's still not stable (for example, the class to define command arguments is `Argument<T>` in the nuget package and `CliArgument<T>` in the repository), mostly works with generic arguments and options (and we abuse of non-generic Type passing around and around...) and has a very different API surface, so migration is not trivial. 
 
The current focus of the project is to get the API for registering and running commands frozen. The roadmap includes adding other nice console interactive features like menus, choices, RGB color support, etc., and also integration with other console "interactive" frameworks like Spectre.Console and PrettyPrompt.
