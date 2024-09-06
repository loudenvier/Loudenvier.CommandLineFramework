using CommandLine;
using Loudenvier.Utils;
//using PrettyPrompt;
using System.Diagnostics;
using System.Runtime.InteropServices;
using YamlDotNet.Serialization;

#pragma warning disable IDE1006 // Naming Styles

namespace Loudenvier.CommandLineFramework;

public class CommandLoop {
    public CommandLoop(ConsolePrinter? printer = null, CommandSet? commandSet = null, ConsoleState? state = null, Parser? parser = null) {
        Printer = printer ?? new();
        /*promptConfig = new PromptConfiguration {
            Prompt = Printer.GetPrompt(),
        };*/
        CommandSet = commandSet ?? new();
        Parser = parser ?? new Parser(with => {
            with.CaseInsensitiveEnumValues = true;
            with.CaseSensitive = CommandSet.Conventions.CaseSensitive;
        });
        State = state ?? new();
        RegisterBuiltInCommands();
    }
    public CommandSet CommandSet { get; }
    public ConsolePrinter Printer { get; }
    public ConsoleState State { get; } 
    public Parser Parser { get; }

    /// <summary>Provides standard serialization services for the command loop. Use it for serialization  
    /// of objects which are displayed in the console.</summary>
    public ISerializer Serializer { get; } = new SerializerBuilder()
        .WithTypeConverter(new Yaml.IPAddressTypeConverter())
        .Build();


    //readonly PromptConfiguration promptConfig;
    public void UpdatePrompt() => SetPrompt(Printer.GetPrompt());
    //public void SetPrompt(string prompt) => promptConfig.Prompt = prompt;
    public void SetPrompt(string prompt) { }
    void RegisterBuiltInCommands() {
        CommandSet.RegisterCommandSet<BuiltInCommands>();
        /*CommandSet
            .RegisterCommand(new VerboseCommand(), ["verbose"])
            .RegisterCommand(new HelpCommand(), ["help"])
            .RegisterCommand(new StateCommand(), ["state"])
            .RegisterCommand(new PromptCommand(), ["prompt"])
            .RegisterCommand(new ClsCommand(), ["cls"]);*/
    }

    bool verbose;
    public bool Verbose { 
        get => verbose;
        set {
            verbose = value;
            State.Set("verbose", value);
            Printer.Info($"Verbose mode is now turned {value.ToString("on", "off")}.");
        }
    }

    static readonly string[] exits = ["exit", "quit", "q"];
    public static IEnumerable<string> ExitCommands => exits;
    static bool IsExitCommand(string cmd) => exits.Contains(cmd, StringComparer.InvariantCultureIgnoreCase);

    /*async Task RunCommand(CommandItem item, string args) {
        var (command, aliases, options, verbs) = item;
        var tokenized = Tokenizer.TokenizeCommandLineToStringArray($"program.exe {args}");
        var parserResult = (options, verbs) switch {
            (not null, _) => Parser.ParseArguments(() => Activator.CreateInstance(options), tokenized),
            (_, { Length: 0 }) => Parser.ParseArguments(() => Activator.CreateInstance(typeof(GenericOptions)), tokenized),
            _ => Parser.ParseArguments(tokenized, verbs)
        };
        var notParsed = false;
        await parserResult.WithNotParsedAsync(async e => {
            notParsed = true;
            await command.Error(e, this);
            Printer.DisplayHelp(parserResult);
        });
        if (!notParsed)
            await command.Execute(parserResult!, this);
    }*/

    async Task Execute(ConsoleInput input) {
        var runner = CommandSet.GetCommandRunner(input.Command);
        if (runner is not null ) {
            try {
                await runner.RunAsync(input.CommandData, this); // RunCommand(runner.Value, input.CommandData);
            } catch (Exception e) {
                Printer.Error($"[{input.Command}] failed with error: {(Verbose ? e.ToStringDemystified() : e.Message)}");
                if (!Verbose)
                    Printer.Error("Set [verbose on] to see full stack trace!");
            }
        } else {
            Printer.Error($"Unknown command: {input.Command}");
            //ShowCommands();
        }
    }

    static ConsoleInput HandleInput(string input) {
        if (string.IsNullOrWhiteSpace(input)) {
            ReadLine.GetHistory().RemoveAt(ReadLine.GetHistory().Count - 1);
            return new ConsoleInput();
        }
        var (key, value) = input.Trim().ToKeyValuePair(normalizeKey: false, trimValue: true, ' ');
        if (IsExitCommand(key))
            return ConsoleInput.Exit;
        return new ConsoleInput {
            Type = InputType.Command,
            Command = key,
            CommandData = value,
        };
    }

    public async Task RunAsync(params string[] args) {
        //var prompt = new Prompt(configuration: promptConfig);
        ReadLine.AutoCompletionHandler = new AutoCompletionHandler(CommandSet.AvailableCommands);
        ReadLine.HistoryEnabled = true;
        ConsoleInput input;
        do {
            Printer.DisplayPrompt();
            //var response = await prompt.ReadLineAsync();
            var response = ReadLine.Read();
            /*input = response.IsSuccess 
                ? HandleInput(response.Text)  //ReadLine.Read());
                : ConsoleInput.Exit; */
            input = string.IsNullOrWhiteSpace(response) 
                ? ConsoleInput.Exit
                : HandleInput(response);
            if (input.Type == InputType.Command)
                await Execute(input);

        } while (input.Type != InputType.Exit);
    }
    public async Task RunCommand(string cmd) {
        var input = HandleInput(cmd);
        await Execute(input);
    }

    class AutoCompletionHandler(string[] availableCommands) : IAutoCompleteHandler
    {
        public string[] Commands { get; } = availableCommands;

        // characters to start completion from
        public char[] Separators { get; set; } = [];
        // text - The current text entered in the console
        // index - The index of the terminal cursor within {text}
        public string[]? GetSuggestions(string text, int index) {
            // apenas enviar sugestões para comandos iniciais
            if (text.Length < 10) {
                var suggestions = Commands
                    .Where(c => c.StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
                    .ToArray();
                if (suggestions.Length > 0)
                    return suggestions;
            }
            return null;
        }
    }
}

#pragma warning restore IDE1006 // Naming Styles
