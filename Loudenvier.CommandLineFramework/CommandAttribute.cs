namespace Loudenvier.CommandLineFramework;

/// <summary>
/// Defines a command with a name and optional aliases.
/// </summary>
/// <param name="name">The name of the command</param>
/// <param name="aliases">An array of possible command aliases</param>
public class CommandAttribute(string name, string[]? aliases = null) : Attribute
{
    /// <summary>
    /// Defines a command with a name and a type that defines
    /// arguments / options for the command and optional aliases 
    /// </summary>
    /// <param name="options">A type that represents the command arguments / options</param>
    /// <param name="name">The name of the command</param>
    /// <param name="aliases">An array of possible command aliases</param>
    public CommandAttribute(Type options, string name, string[]? aliases = null):this(name, aliases) {
        Options = options;
    }
    public CommandAttribute(string name, Type verbsContainer, string[]? aliases = null) : this(name, aliases) {
        VerbsContainer = verbsContainer;
    }
    public CommandAttribute(string name, Type[] verbs, string[]? aliases = null) : this(name, aliases) {
        Verbs = verbs ?? [];
    }
    public string Name { get; } = name;
    public string[] Aliases { get; } = aliases ?? [];
    public Type? Options { get; }
    public Type[] Verbs { get; } = [];
    public Type? VerbsContainer { get; }
}
