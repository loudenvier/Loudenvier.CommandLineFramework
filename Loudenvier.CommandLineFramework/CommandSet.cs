using CommandLine;
using System.Reflection;

namespace Loudenvier.CommandLineFramework;

public class CommandSet {
    public CommandSet(bool autoRegisterCommands = true, CommandConventions? conventions = null) {
        Conventions = conventions ?? CommandConventions.Default;
        if (autoRegisterCommands)
            AutoRegisterCommands();
    }

    public CommandConventions Conventions { get; } 

    readonly Dictionary<string, CommandRunner> cmds = [];
    public string[] AvailableCommands => [.. cmds.Keys];

    public CommandRunner? GetCommandRunner(string cmd)
        => cmds.TryGetValue(cmd, out var item) ? item : null;

    static readonly BindingFlags bindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

    public void RegisterCommandSet<T>() => RegisterCommandSet(typeof(T));
    public void RegisterCommandSet(Type setType) {
        var methods = setType.GetMethods(bindings);
        var types = setType.GetNestedTypes(bindings);
        var explicitCommands = methods.Where(m => m.GetCustomAttributes<CommandAttribute>().Any());
        var implicitCommands = methods.Where(m => m.Name.EndsWith(Conventions.CommandSuffix));
        var implicitOptions = types.Where(t => t.Name.EndsWith(Conventions.OptionsSuffix));
        var implicitVerbSet = types.Where(t => t.Name.EndsWith(Conventions.VerbSetSuffix));
        //var implicitVerbs = types.Where(t => t.GetCustomAttribute<VerbAttribute>() != null);

        foreach (var method in explicitCommands) {
            var commandAttr = method.GetCustomAttribute<CommandAttribute>();
            var verbs = commandAttr.Verbs;
            if (verbs.Length == 0 && commandAttr.VerbsContainer != null) {
                verbs = commandAttr.VerbsContainer
                    .GetNestedTypes()
                    .Where(t => t.GetCustomAttribute<VerbAttribute>() != null)
                    .ToArray();
            }
            var runner = new CommandRunner(
                method,
                commandAttr.Aliases,
                commandAttr.Options,
                verbs);
            string[] names = [.. new string[] { commandAttr.Name }, .. commandAttr.Aliases];
            foreach (var name in names)
                cmds[name] = runner;
        }
        foreach (var method in implicitCommands) {
            var name = Conventions.GetCommandName(method.Name);
            var options = implicitOptions
                .Where(o => o.Name == $"{name}{Conventions.OptionsSuffix}")
                .SingleOrDefault();
            var verbSet = implicitVerbSet
                .Where(v => v.Name == $"{name}{Conventions.VerbSetSuffix}")
                .SingleOrDefault();
            var verbs = verbSet?
                .GetNestedTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null)
                .ToArray() ?? [];
            var runner = new CommandRunner(
                method,
                [],
                options,
                verbs);
            cmds[name] = runner;
        }
    }
    public void AutoRegisterCommands() {
        var commanSets = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(asm => asm.GetTypes().Where(t => t.Name.EndsWith(Conventions.CommandSetSuffix)));
        foreach (var set in commanSets) {
            RegisterCommandSet(set);
            //var cmdAttr = cmdType.GetCustomAttribute<ConsoleCommandAttribute>()!;
            //var command = (ConsoleCommand)Activator.CreateInstance(cmdType)!;
            //RegisterCommand(command, cmdAttr.Names);
        }
    }
}
