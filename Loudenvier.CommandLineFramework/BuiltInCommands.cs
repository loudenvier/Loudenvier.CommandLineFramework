using CommandLine;
using static Loudenvier.CommandLineFramework.BuiltInCommands.VerboseVerbs;

namespace Loudenvier.CommandLineFramework;

#pragma warning disable IDE0051 // Remove unused private members
class BuiltInCommands
{
    static string PromptDesc => "Gets or sets the current console prompt";
    static void PromptCommand(GenericOptions o, CommandLoop loop) {
        loop.Printer.Prompt = string.IsNullOrWhiteSpace(o.Text) ? "cmd" : o.Text;
        loop.UpdatePrompt();
    }

    [Command("cls", Description = "Clears the console screen")]
    static void Cls(object _, CommandLoop loop) => loop.Printer.Clear();

    class HelpOptions {
        [Value(0, MetaName = "command", Required = false)]
        public IEnumerable<string>? Args { get; set; }
        public string Command => string.Join(" ", Args ?? []);
    }
    static string HelpDesc => "Displays available commands or help for a specific command";
    static async Task HelpCommand(HelpOptions o, CommandLoop loop) {
        if (string.IsNullOrWhiteSpace(o.Command)) {
            string[] exits = [string.Join("/", CommandLoop.ExitCommands)];
            //var commands = string.Join(",", [.. loop.CommandSet.AvailableCommands, .. exits]);
            var commands = exits.Concat(loop.CommandSet.AvailableCommands.OrderBy(s => s));
            loop.Printer.Info($"Available commands:\r\n");
            foreach (var command in commands) {
                var cmd = loop.CommandSet.GetCommandRunner(command);
                if (cmd != null)
                    loop.Printer.Label($"  {command, 20} ... {cmd.Description, -54}");
                else
                    loop.Printer.Label($"  {command, 20}");
            }
            loop.Printer.Label("");
        } else {
            await loop.RunCommand($"{o.Command} --help");
        }
    }

    static string StateDesc => "Displays the current state of the console";
    static void StateCommand(object _, CommandLoop loop) {
        var yaml = loop.Serializer.Serialize(loop.State);
        loop.Printer.Info(yaml);
    }

    static string VerboseDesc => "Gets or sets the console verbosity (on, off)";
    public class VerboseVerbs 
    {
        [Verb("on")]
        public class VerbOn { }
        [Verb("off")]
        public class VerbOff { }
        [Verb("status", isDefault: true)]
        public class VerbStatus { }
    }
    static void VerboseCommand(object result, CommandLoop loop) {
        loop.Verbose = result switch {
            VerbOn => true,
            VerbOff => false,
            _ => loop.Verbose,
        };
    }
}
#pragma warning restore IDE0051 // Remove unused private members
