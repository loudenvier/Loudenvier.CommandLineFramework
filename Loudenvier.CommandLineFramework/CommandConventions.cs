namespace Loudenvier.CommandLineFramework;

public record class CommandConventions(
    string CommandSetSuffix = "CommandSet",
    string CommandSuffix = "Command",
    string OptionsSuffix = "Options",
    string VerbSetSuffix = "Verbs",
    bool CaseSensitive = false,
    CommandCase Casing = CommandCase.Original) 
{
    public static CommandConventions Default { get; set; } = new();

    public string GetCommandName(string text) => (Casing switch {
        _ => text,
    })[.. (text.Length - CommandSuffix.Length)];
};
