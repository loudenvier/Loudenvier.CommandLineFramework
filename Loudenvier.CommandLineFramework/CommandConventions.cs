using Loudenvier.Utils;

namespace Loudenvier.CommandLineFramework;

public record class CommandConventions(
    string CommandSetSuffix = "CommandSet",
    string CommandSuffix = "Command",
    string OptionsSuffix = "Options",
    string VerbSetSuffix = "Verbs",
    bool CaseSensitive = false,
    CaseConvention Casing = CaseConvention.Original) 
{
    public static CommandConventions Default { get; set; } = new();

    public string GetCommandName(string text) 
        => Casing.ApplyCase(text[..(text.Length - CommandSuffix.Length)]);
};
