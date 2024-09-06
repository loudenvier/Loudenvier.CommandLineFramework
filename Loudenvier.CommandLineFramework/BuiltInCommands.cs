using CommandLine;
using static Loudenvier.CommandLineFramework.BuiltInCommands.VerboseVerbs;

namespace Loudenvier.CommandLineFramework;

#pragma warning disable IDE0051 // Remove unused private members
class BuiltInCommands
{
    static void PromptCommand(GenericOptions o, CommandLoop loop) {
        loop.Printer.Prompt = string.IsNullOrWhiteSpace(o.Text) ? "cmd" : o.Text;
        loop.UpdatePrompt();
    }

    static void ClsCommand(object _, CommandLoop loop) => loop.Printer.Clear();

    class HelpOptions {
        [Value(0, MetaName = "command", Required = false)]
        public IEnumerable<string>? Args { get; set; }
        public string Command => string.Join(" ", Args ?? []);
    }
    static async Task HelpCommand(HelpOptions o, CommandLoop loop) {
        if (string.IsNullOrWhiteSpace(o.Command)) {
            string[] exits = [string.Join("/", CommandLoop.ExitCommands)];
            var commands = string.Join(",", [.. loop.CommandSet.AvailableCommands, .. exits]);
            loop.Printer.Info($"Available commands: {commands}");
        } else {
            await loop.RunCommand($"{o.Command} --help");
        }
    }

    static void StateCommand(object _, CommandLoop loop) {
        var yaml = loop.Serializer.Serialize(loop.State);
        loop.Printer.Info(yaml);
    }

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
