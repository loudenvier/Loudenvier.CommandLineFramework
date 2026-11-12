using CommandLine;
using System.Reflection;

namespace Loudenvier.CommandLineFramework;

public class CommandSet {
    public CommandSet(bool autoRegisterCommands = true, CommandConventions? conventions = null) {
        Conventions = conventions ?? CommandConventions.Default;
        cmds = new(Conventions.CaseSensitive 
            ? StringComparer.InvariantCulture 
            : StringComparer.InvariantCultureIgnoreCase);
        if (autoRegisterCommands)
            AutoRegisterCommands();
    }

    public CommandConventions Conventions { get; }

    readonly Dictionary<string, CommandRunner> cmds;
    public string[] AvailableCommands => [.. cmds.Keys];

    public CommandRunner? GetCommandRunner(string cmd)
        => cmds.TryGetValue(cmd, out var item) ? item : null;

    static readonly BindingFlags bindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

    public void RegisterCommandSet<T>() => RegisterCommandSet(typeof(T));
    public void RegisterCommandSet(Type setType) {
        var methods = setType.GetMethods(bindings);
        var types = setType.GetNestedTypes(bindings);
        var props = setType.GetProperties(bindings);
        var explicitCommands = methods.Where(m => m.IsDefined(typeof(CommandAttribute)));
        var implicitCommands = methods.Where(m => m.Name.EndsWith(Conventions.CommandSuffix)
            && !m.IsDefined(typeof(CommandAttribute)));
        var implicitOptions = types.Where(t => t.Name.EndsWith(Conventions.OptionsSuffix));
        var implicitVerbSet = types.Where(t => t.Name.EndsWith(Conventions.VerbSetSuffix));

        // Check every name before updating the registry, including aliases within this set.
        var pending = new Dictionary<string, CommandRunner>(cmds.Comparer);
        void RegisterName(string name, CommandRunner runner) {
            if ((pending.TryGetValue(name, out var existing) || cmds.TryGetValue(name, out existing))
                && !existing.Method.Equals(runner.Method)) {
                throw new InvalidOperationException(
                    $"Command name or alias '{name}' is already registered for "
                    + $"'{existing.Method.DeclaringType?.FullName}.{existing.Method}'. "
                    + $"Cannot register '{runner.Method.DeclaringType?.FullName}.{runner.Method}' with the same name.");
            }

            pending[name] = runner;
        }

        foreach (var method in explicitCommands) {
            var commandAttr = method.GetCustomAttribute<CommandAttribute>()!;
            var verbs = commandAttr.Verbs;
            if (verbs.Length == 0 && commandAttr.VerbsContainer != null) {
                verbs = [.. commandAttr.VerbsContainer
                    .GetNestedTypes(bindings)
                    .Where(t => t.GetCustomAttribute<VerbAttribute>() != null)];
            }
            var runner = new CommandRunner(
                method,
                commandAttr.Aliases,
                commandAttr.Options,
                verbs,
                commandAttr.Description);
            RegisterName(commandAttr.Name, runner);
            foreach (var alias in commandAttr.Aliases)
                RegisterName(alias, runner);
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
                .GetNestedTypes(bindings)
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null)
                .ToArray() ?? [];
            var descProp = props.SingleOrDefault(p => p.Name == $"{name}Desc");
            var desc = descProp != null
                ? (string)descProp.GetValue(null)
                : null;
            var runner = new CommandRunner(
                method,
                [],
                options,
                verbs,
                desc);
            RegisterName(name, runner);
        }

        foreach (var entry in pending)
            cmds[entry.Key] = entry.Value;
    }
    public void AutoRegisterCommands() {
        var commanSets = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(asm => GetLoadableTypes(asm))
            .Where(t => t.Name != "CommandSet" && t.Name.EndsWith(Conventions.CommandSetSuffix));

        foreach (var set in commanSets) {
            RegisterCommandSet(set);
        }
    }
    private IEnumerable<Type> GetLoadableTypes(Assembly assembly) {
        try {
            return assembly.GetTypes();
        } catch (ReflectionTypeLoadException e) {
            // O LINQ OfType<Type>() filtra automaticamente os itens nulos 
            // e devolve apenas as classes que o .NET conseguiu carregar com sucesso.
            return e.Types.OfType<Type>();
        }
    }
}
