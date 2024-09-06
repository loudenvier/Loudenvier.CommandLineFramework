using CommandLine;

namespace Loudenvier.CommandLineFramework;

public class GenericOptions
{
    [Value(0, MetaName = "options", HelpText = "Options for the command (use ordinary command line syntax)")]
    public IEnumerable<string>? Args { get; set; }
    public string Text => string.Join(" ", Args);
}
