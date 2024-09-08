using CommandLine;
using Loudenvier.CommandLineFramework.Parsing;
using System.Reflection;

namespace Loudenvier.CommandLineFramework;

public class CommandRunner {
    public CommandRunner(MethodInfo method, string[] aliases, Type? options, Type[] verbs) {
        Method = method;
        Aliases = aliases;
        Options = options;
        Verbs = verbs;
        var returnType = method.ReturnType;
        // currently we only check if it's a Task (there maybe other awaitable edge cases out there...)
        IsAwaitable = typeof(Task).IsAssignableFrom(returnType);
        IsVoid = returnType == typeof(void);
    }
    public MethodInfo Method { get; }
    public bool IsVoid { get; }
    public bool IsAwaitable { get; }
    public string[] Aliases { get; }
    public Type? Options { get; }
    public Type[] Verbs { get; }    

    async Task Invoke(object result, CommandLoop loop) {
        var methodParams = Method.GetParameters();
        object[] parameters = methodParams.Length switch {
            0 => [],
            1 => methodParams[0].ParameterType == typeof(CommandLoop) ? [loop] : [result],
            2 => methodParams[0].ParameterType == typeof(CommandLoop) ? [loop, result] : [result, loop],
            _ => throw new Exception("The command parameter list is invalid. There are more than 2 parameters! A command may have no parameter, or 1 parameter of either the option type or the CommandLoop type, or 2 parameters: one option type and one CommandLoop type (in any order).")
        };
        if (IsVoid) {
            Method.Invoke(null, parameters);
            await Task.CompletedTask;
        } else if (IsAwaitable) {
            var task = (Task)Method.Invoke(null, parameters);
            await task;
        } else {
            throw new Exception("A command cannot be void and awaitable at the same time!");
        }
    }

    public async Task RunAsync(string args, CommandLoop loop) {
        var tokenized = Tokenizer.TokenizeCommandLineToStringArray($"program.exe {args}");
        var parserResult = (Options, Verbs) switch {
            (not null, _) => loop.Parser.ParseArguments(() => Activator.CreateInstance(Options), tokenized),
            (_, { Length: 0 }) => loop.Parser.ParseArguments(() => Activator.CreateInstance(typeof(GenericOptions)), tokenized),
            _ => loop.Parser.ParseArguments(tokenized, Verbs)
        };
        if (parserResult.Tag == ParserResultType.Parsed) {
            var result = parserResult.Value;
            await Invoke(result, loop);
        } else {
            loop.Printer.DisplayHelp(parserResult);
        }
    }
}
